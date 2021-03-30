using UnityEngine;
using System.Collections.Generic;

// Handles object/JSON conversion
public static class NetworkUtil {

	// Parse incoming JSON positions from server
	public static EcgMessageRaw JsonToEcgMessageRaw(string json) {
        EcgMessageRaw msg = JsonUtility.FromJson<EcgMessageRaw>(json);
		return msg;
	}

}
