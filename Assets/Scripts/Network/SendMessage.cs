using UnityEngine;
using System.Collections.Generic;

public class SendMessage : MonoBehaviour {

	///////////////////////////////////////////////////////////////////////
	// This class will broadcast the Transform data of a single User/Player.
	////////////////////////////////////////////////////////////////////////

	// Make sure to point this to your Network Manager instance
	public NetworkManager netManager;

	// This class is set up to manage one set of three Transforms
	// (head, L-hand, and R-hand).
	public Transform headTransform;
	public Transform leftHandTransform;
	public Transform rightHandTransform;

	// I've experimented with throttling how often data is sent. In practice,
	// since we're using a local network this shouldn't be necessary.
	public bool rateLimit = false;
	public int limitFactor = 3;

	// We'll get these from the Network Manager at runtime
	private bool showDebug;
	private int uniqueID;

	private void Start() {
		// Set these vars from the Network Manager
		showDebug = netManager.showDebug;
		uniqueID = netManager.uniqueID;
	}

	private void Update () { 
		// You can implement some sort of guard/check here if you want to wait until
		// certain things in your scene have initialized before you proceed any further
		// and send positional data.

		if (rateLimit) {
			if (Time.frameCount % limitFactor == 0) {
				netManager.SendPosData(NetworkUtil.ConvertUserPosToJSON(uniqueID, headTransform, leftHandTransform, rightHandTransform));
			}
		} else {
			netManager.SendPosData(NetworkUtil.ConvertUserPosToJSON(uniqueID, headTransform, leftHandTransform, rightHandTransform));
		}
	}

}
