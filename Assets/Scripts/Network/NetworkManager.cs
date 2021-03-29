using System;
using UnityEngine;
using BestHTTP.SocketIO;

public class NetworkManager : MonoBehaviour {

	///////////////////////////////////////////////////////////////////////
	// This is the Network Manager that takes care of communicating with the
	// node server. It depends on the BestHTTP Pro package from the unity store.
	////////////////////////////////////////////////////////////////////////

	// Set the network ID of this machine (Can be any unique int)
	public int uniqueID;

	// Set the local network address and port of the server
	public string localServerAddress = "localhost";
	public string localSeverPort = "8008";

	// Point this to wherever you've got the ReceivePositions script.
	public ReceivePositions[] positionReceiver;

	public ReceiveTriggers triggerReceiver;

	// Enable/disable debugging
	public bool showDebug = true;

	// Configure socket.io
	SocketManager socketManager;
	private string socketAddr;
	private bool connectedToServer = false;

	public bool getNetStatus() {
		return connectedToServer;
	}

    private void Start () {
		socketAddr = "http://" + localServerAddress + ":" + localSeverPort + "/socket.io/:8443";
		initSocketManager(socketAddr);
	}

	private void initSocketManager(string uri) {
		socketManager = new SocketManager(new Uri(uri));
		socketManager.Socket.AutoDecodePayload = false;
		socketManager.Socket.On("error", SocketError);
		socketManager.Socket.On("connect", SocketConnected);
		socketManager.Socket.On("reconnect", SocketConnected);

		// If you define new socket routes, make sure to register them here:
		socketManager.Socket.On("objectPositions", ReceivedLocalSocketMessage);
		socketManager.Socket.On("trigger", ReceivedLocalSocketMessage);
	}

	// Confirm that socket has successfully connected
	private void SocketConnected(Socket socket, Packet packet, params object[] args) {
		connectedToServer = true;

		if (showDebug) {
			Debug.Log(DateTime.Now + " - " + "Success connecting to sockets.");
		}
	}

	// Print Socket Errors
	private void SocketError(Socket socket, Packet packet, params object[] args) {
		connectedToServer = false;
		if (showDebug) {
			Debug.LogError(DateTime.Now + " Error connecting to sockets.");

			if (args.Length > 0) {
				Error error = args[0] as Error;
				if (error != null) {
					switch (error.Code) {
						case SocketIOErrors.User:
							Debug.LogError("Socket Error Type: Exception in an event handler.");
							break;
						case SocketIOErrors.Internal:
							Debug.LogError("Socket Error Type: Internal error.");
							break;
						default:
							Debug.LogError("Socket Error Type: Server error.");
							break;
					}
					Debug.LogError(error.ToString());
					return;
				}
			}
			Debug.LogError("Could not Parse Error.");
		}
	}

	// Do stuff with the incoming socket data
	private void ReceivedLocalSocketMessage(Socket socket, Packet packet, params object[] args) {
		// This bit parses and formats the incoming data so that we can use
		// Unity's built-in JSON parser
		string eventName = "data";
		string jsonString;
		if (packet.SocketIOEvent == SocketIOEventTypes.Event) {
			eventName = packet.DecodeEventName();
			jsonString = packet.RemoveEventName(true);
		} else if (packet.SocketIOEvent == SocketIOEventTypes.Ack) {
			jsonString = packet.ToString();
			jsonString = jsonString.Substring(1, jsonString.Length-2);
		} else {
			jsonString = packet.ToString();
		}

		// Uncomment this to show the raw incoming data from the
		// server as part of your debug dump:
		/*
		if (showDebug) {
			Debug.Log(DateTime.Now + " - " + "Local Socket Event Name: " + eventName + " - Message: " + jsonString);
		}
		*/

		// Look for the socket event name and do something with it here
		// *** Remember to register your event names in initSocketManager() ***
		switch (eventName) {
			case "objectPositions":
				// Send the JSON string to the position receiver
				for (int i=0; i<positionReceiver.Length; i++) {
					positionReceiver[i].UpdatePositionsFromJson(jsonString);
				}
				break;

			case "trigger":
				// Just an example. Work your own magic here :-D
				//Debug.Log("Got Trigger from: " + ParseTriggerSource(jsonString) );
				//Debug.Log(jsonString);
				triggerReceiver.ProcessTrigger(jsonString);
				break;
		}
	}

	// This is called from SendPosition.cs
	public void SendPosData(string posData) {
		// Here you are sending to the server. Make sure that the server
		// is listening for this event name.
		socketManager.Socket.Emit("newPosData", posData);
	}

	// This can be called from anywhere
	public void SendTrigger(string triggerData) {
		// Here you are sending to the server. Make sure that the server
		// is listening for this event name.

		//print(triggerData);
		socketManager.Socket.Emit("trigger", triggerData);
	}

	// Helpers and cleanup

	// Try to an int out of the incoming trigger payload, which
	// should just be the sending ID cast to a string.
	private int ParseTriggerSource(string incoming) {
		int incomingId;
		bool success = Int32.TryParse(incoming, out incomingId);
		if (success) { return incomingId; }

		if (showDebug) {
			Debug.LogWarning("Could not get source ID of incoming trigger");
		}
		return -1;
	}

	// Try to gracefully close the socket connection
	private void OnApplicationQuit() {
		socketManager.Close();
		if (showDebug) {
			Debug.Log("Closed connection");
		}
	}
    
}
