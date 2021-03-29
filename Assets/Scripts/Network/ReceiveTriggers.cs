using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveTriggers : MonoBehaviour {

	public NetworkManager netManager;
	//public SoundManager soundManager;

	private bool showDebug;
	private int uniqueID;

	[HideInInspector] public GameObject[] poiArray;

	private void Start () {
		showDebug = netManager.showDebug;
		uniqueID = netManager.uniqueID;
	}

	public void CreatePOIList() {
		poiArray = GameObject.FindGameObjectsWithTag("POI");
		Debug.Log("POI Array count:" + poiArray.Length);
	}

	public void ProcessTrigger(string json) {
		TriggerData td = JsonHelpers.JSONToTriggerData(json);
		// Debug.Log(td.playerId + " " + td.cueId + " " + td.status);

		if (td.playerId != uniqueID) {
			for (int i = 0; i < poiArray.Length; i++) {
				/*
				if (poiArray[i].GetComponent<POIActivator>().index == td.cueId) {
					POIActivator poi = poiArray[i].GetComponent<POIActivator>();
					if (td.status) {
						poi.active = true;
						poi.onNetActivate(true);
					} else {
						poi.active = false;
						poi.onNetActivate(false);
					}
				}
				*/
			}
		}
	}

}
