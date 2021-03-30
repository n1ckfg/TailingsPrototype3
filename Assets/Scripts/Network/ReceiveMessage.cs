using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveMessage : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////
    // This is built in a totally non-scalable way, to make it
    // easier to understand and then change as needed.
    ////////////////////////////////////////////////////////////////////////

    public enum LerpPositions { NONE, POS, ROT, POS_ROT };
    public LerpPositions lerpPositions = LerpPositions.POS_ROT;

    // Make sure to point this to your Network Manager instance
    public NetworkManager netManager;

    // The ID to listen for (the ID of the incoming position data)
    // and its corresponding transforms. I think it would be best to
    // wrap this in a way where you instantiate new elements as you
    // detect new ID's, but this hard-coded way should be clear enough
    // to modify.
    public int listenForThisID;
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public float smooth = 0.5f;

    private Vector3 headPos;
    private Quaternion headRot;
    private Vector3 leftHandPos;
    private Quaternion leftHandRot;
    private Vector3 rightHandPos;
    private Quaternion rightHandRot;

    // We'll get these from the Network Manager at runtime:
    private bool showDebug;
    private int uniqueID;

    private void Start() {
        // Set these vars from the Network Manager:
        showDebug = netManager.showDebug;
        uniqueID = netManager.uniqueID;

        headPos = new Vector3(headTransform.position.x, headTransform.position.y, headTransform.position.z);
        headRot = new Quaternion(headTransform.rotation.x, headTransform.rotation.y, headTransform.rotation.z, headTransform.rotation.w);

        leftHandPos = new Vector3(leftHandTransform.position.x, leftHandTransform.position.y, leftHandTransform.position.z);
        leftHandRot = new Quaternion(leftHandTransform.rotation.x, leftHandTransform.rotation.y, leftHandTransform.rotation.z, leftHandTransform.rotation.w);

        rightHandPos = new Vector3(rightHandTransform.position.x, rightHandTransform.position.y, rightHandTransform.position.z);
        rightHandRot = new Quaternion(rightHandTransform.rotation.x, rightHandTransform.rotation.y, rightHandTransform.rotation.z, rightHandTransform.rotation.w);
    }

    private void Update() {
        if (lerpPositions == LerpPositions.POS || lerpPositions == LerpPositions.POS_ROT) {
            headTransform.position = Vector3.Lerp(headTransform.position, headPos, smooth);
            leftHandTransform.position = Vector3.Lerp(leftHandTransform.position, leftHandPos, smooth);
            rightHandTransform.position = Vector3.Lerp(rightHandTransform.position, rightHandPos, smooth);
        } else {
            headTransform.position = headPos;
            leftHandTransform.position = leftHandPos;
            rightHandTransform.position = rightHandPos;
        }

        if (lerpPositions == LerpPositions.ROT || lerpPositions == LerpPositions.POS_ROT) {
            headTransform.rotation = Quaternion.Lerp(headTransform.rotation, headRot, smooth);
            leftHandTransform.rotation = Quaternion.Lerp(leftHandTransform.rotation, leftHandRot, smooth);
            rightHandTransform.rotation = Quaternion.Lerp(rightHandTransform.rotation, rightHandRot, smooth);
        } else {
            headTransform.rotation = headRot;
            leftHandTransform.rotation = leftHandRot;
            rightHandTransform.rotation = rightHandRot;
        }
    }

    // This is called from the network manager
    public void UpdateDataFromJson(string json) {
        List<UserPosition> positions = NetworkUtil.JSONToUserPosList(json);

        // Don't do anything if you're receiving empty data.
        if (positions.Count == 0) {
            if (showDebug) {
                Debug.LogWarning("Position array from server was empty");
            }
            return;
        }
    }

}
