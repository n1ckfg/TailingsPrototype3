using UnityEngine;
using System.Collections.Generic;

// Handles object/JSON conversion
public static class NetworkUtil {

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
