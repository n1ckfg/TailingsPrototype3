using UnityEngine;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////
// This file defines the serializable data types that we're sending
// back and forth to the server
////////////////////////////////////////////////////////////////////////

// Holds the data of a single Unity GameObject.Transform, plus an id
[System.Serializable]
public struct SinglePosition {
	public int id;
	public float posX;
	public float posY;
	public float posZ;
	public float rotX;
	public float rotY;
	public float rotZ;
	public float rotW;
}

// I made this struct to hold the information of one "Player" consisting
// of a userID and a List (which becomes Array in JSON) of Transforms
[System.Serializable]
public struct UserPosition {
	public int userID;
	public List<SinglePosition> positions;
}

// This is needed to properly parse the incoming JSON with multiple
// UserPositions from the server
[System.Serializable]
public struct IncomingPositions {
	public List<UserPosition> userPosList;
}

[System.Serializable]
public struct TriggerData {
	public int playerId;
	public int cueId;
	public bool status;
}
