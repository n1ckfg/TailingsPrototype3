using UnityEngine;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////
// This file defines the Serializable data types that we're sending
// back and forth to the server, as well as a class 'JsonHelpers' with
// static methods to abstract the object/JSON conversion.
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

// Defines a static class to abstract object/JSON conversion
public static class JsonHelpers {

	public static SinglePosition ConvertToSinglePosObj(int id, Transform trans) {
		SinglePosition singlePos;
		singlePos.id = id;
		singlePos.posX = trans.position.x;
		singlePos.posY = trans.position.y;
		singlePos.posZ = trans.position.z;
		singlePos.rotX = trans.rotation.x;
		singlePos.rotY = trans.rotation.y;
		singlePos.rotZ = trans.rotation.z;
		singlePos.rotW = trans.rotation.w;
		return singlePos;
	}

	public static string ConvertUserPosToJSON(int id, Transform head, Transform leftHand, Transform rightHand) {
		string json;
		UserPosition userPos;
		userPos.userID = id;
		userPos.positions = new List<SinglePosition>();

		SinglePosition singlePosHead = ConvertToSinglePosObj(0, head);
		userPos.positions.Add(singlePosHead);

		SinglePosition singlePosLeft = ConvertToSinglePosObj(1, leftHand);
		userPos.positions.Add(singlePosLeft);

		SinglePosition singlePosRght = ConvertToSinglePosObj(2, rightHand);
		userPos.positions.Add(singlePosRght);

		json = JsonUtility.ToJson(userPos);
		return json;
	}

	// Parse incoming JSON positions from server
	public static List<UserPosition> JSONToUserPosList(string json) {
		IncomingPositions incoming = JsonUtility.FromJson<IncomingPositions>(json);
		return incoming.userPosList;
	}

	public static string ConvertTriggerDataToJSON(TriggerData td) {
		string json;
		json = JsonUtility.ToJson(td);
		return json;
	}

	// Parse incoming JSON triggers from server
	public static TriggerData JSONToTriggerData(string json) {
		TriggerData td = JsonUtility.FromJson<TriggerData>(json);
		return td;
	}
}
