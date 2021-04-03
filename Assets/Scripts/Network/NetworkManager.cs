using System;
using UnityEngine;
using BestHTTP.SocketIO;

public class NetworkManager : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////
    // The Network Manager handles communication with the node server. 
    // It depends on the BestHTTP Pro package from the Unity store.
	////////////////////////////////////////////////////////////////////////

	// Set the network ID of this machine (Can be any unique int)
	public int uniqueId;

	// Set the local network address and port of the server
	public string serverAddress = "localhost";

	// Point this to wherever you've got the ReceivePositions script.
	public NetworkReceiver[] receivers;
	public NetworkWsVideo wsVideoReceiver;

	// Enable/disable debugging
	public bool showDebug = true;

	// Configure socket.io
	SocketManager socketManager;
	private string socketAddress;
	private bool connectedToServer = false;

	public bool getNetStatus() {
		return connectedToServer;
	}

    private void Start () {
        // There are a lot of different ways to format a websocket url.
        // This is meant for communication between a Unity WebGL client and a remote node.js server.
        // *** Socket.io 2 is assumed. ***
        socketAddress = "https://" + serverAddress + "/socket.io/";

        initSocketManager(socketAddress);
	}

	private void initSocketManager(string uri) {
		socketManager = new SocketManager(new Uri(uri));
		socketManager.Socket.AutoDecodePayload = false;
		socketManager.Socket.On("error", SocketError);
		socketManager.Socket.On("connect", SocketConnected);
		socketManager.Socket.On("reconnect", SocketConnected);

		// *** If you define new socket routes, make sure to register them here! ***
		socketManager.Socket.On("broadcast", ReceivedLocalSocketMessage);
		socketManager.Socket.On("video", ReceivedLocalSocketMessage);
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

	// Use the incoming socket data
	private void ReceivedLocalSocketMessage(Socket socket, Packet packet, params object[] args) {
		// This checks the incoming data so it's compatible with Unity's built-in JSON parser
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

		// Uncomment this to show the raw incoming data from the server as part of your debug dump:
		/*
		if (showDebug) {
			Debug.Log(DateTime.Now + " - " + "Local Socket Event Name: " + eventName + " - Message: " + jsonString);
		}
		*/

		// Look for the socket event name and do something with it here
		// *** Remember to register your event names in initSocketManager() ***
		switch (eventName) {
			case "broadcast":
                // Send the new data to the correct receiver
                EcgMessageRaw msg1 = JsonUtility.FromJson<EcgMessageRaw>(jsonString);

                int index = msg1.player_index - 1;
                if (index >= 0 && index < receivers.Length) {
                    receivers[index].UpdateData(msg1);
                }
                break;
			case "video":
				VideoMessage msg2 = JsonUtility.FromJson<VideoMessage>(jsonString);
				Debug.Log("Received video from: " + msg2.unique_id);
				wsVideoReceiver.UpdateData(msg2);
				break;
		}
	}

	// Gracefully close the socket connection
	private void OnApplicationQuit() {
		socketManager.Close();
		if (showDebug) {
			Debug.Log("Closed connection");
		}
	}
    
}
