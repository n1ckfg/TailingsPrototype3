#!/bin/env node

const express	= require('express');
const app	= express();
const server = require('http').Server(app);
const io = require('socket.io')(server);
const bodyParser = require('body-parser');
const fs = require('fs');

let env = process.env.NODE_ENV || 'dev';
let objectPositions = {};

let updatePosTime = 33;		// Approx. 30fps;
//let updatePosTime = 17;	// Approx. 60fps;

let updatePosInterval = setInterval(function() {
	sendAllPositions();
}, updatePosTime);
//clearInterval(updatePosTime);

// GET routes
// Go to these from any browser on the network
app.use(require('express').static(__dirname + '/public'));

app.use( bodyParser.json() );
app.use(bodyParser.urlencoded({
	extended: true
}));

// Check status
app.get('/', function(req, res) {
	res.send({"status": "OK", "environment": env});
});

// Clear old data (if you don't want to restart server to do this)
app.get('/clear', function(req, res) {
	objectPositions = {};
	res.send({"status": "OK", "environment": env, "cleared": true});
});

// Output snapshot of position data
app.get('/positions', function(req, res) {
	res.send({"status": "OK", "environment": env, "positions": objectPositions});
});

// Explicitly mark all other routes as illegal
app.get('/*', function(req, res) {
	res.status(400).send('Bad Request');
});

// Sockets
io.on('connection', function(socket) {
	console.log('a user connected');

	socket.on('disconnect', function(){
		console.log('user disconnected');
	});

	// This route is pretty much where all the mojo happens
	socket.on('newPosData', function(data) {
		processIncoming(JSON.parse(data));
	});

	// An example of a route that sends the data it receives back out to all clients
	socket.on('trigger', function(data) {
		io.emit('trigger', JSON.parse(data));
	});
});

// Functions
function processIncoming(objectPos) {
	objectPositions[objectPos.id] = objectPos;

	// You can experiment with updating ALL clients whenever ANY client
	// sends new data. In practice, I think this is overkill.
	//sendAllPositions();
}

function sendAllPositions() {
	// Format the outgoing Json to be nice to the Unity parser
	let objectArray = [];
	for (let key in objectPositions) {
		if (objectPositions.hasOwnProperty(key)) {
			objectArray.push(objectPositions[key])
		}
	}


	// Because we're sending positional data so frequently, we can send
	// the payload as 'volatile' and therefore tolerate some lost packets.
	io.volatile.emit('objectPositions', {"userPosList":objectArray});
}

// Cleanup on close
function terminator(sig){
	if (typeof sig === "string") {
		console.log('Received ' + sig + ' - terminating app ...')
		process.exit(1);
	}
	console.log('Node server stopped.')
};

process.on('exit', function() { terminator(); });

['SIGHUP', 'SIGINT', 'SIGQUIT', 'SIGILL', 'SIGTRAP', 'SIGABRT',
 'SIGBUS', 'SIGFPE', 'SIGUSR1', 'SIGSEGV', 'SIGTERM'
].forEach(function(element, index, array) {
		process.on(element, function() { terminator(element); });
});

// Play nicely with nodemon when developing:
if (env === "production") {
	process.on('SIGUSR2', function() { terminator('SIGUSR2'); });
}

// Start server
// let ipaddress = process.env.OPENSHIFT_NODEJS_IP || "Escape-Pod.local"; //use to be visible on network
// let ipaddress = process.env.OPENSHIFT_NODEJS_IP || "10.0.10.12"; //use to be visible on network
let ipaddress = process.env.ARSYNC_IP || "0.0.0.0";
let port = process.env.ARSYNC_PORT || 8008;

if (typeof ipaddress !== process.env.ARSYNC_IP) {
	// Log errors on OpenShift but continue w/ 127.0.0.1 - this
	// allows us to run/test the app locally.
	console.warn('No ARSYNC_IP let, using \'%s\'',ipaddress);
};

server.listen(port, ipaddress, function () {
	let host = server.address().address
	let port = server.address().port
	console.log('Node server started on %s:%d ...', host, port);
});
