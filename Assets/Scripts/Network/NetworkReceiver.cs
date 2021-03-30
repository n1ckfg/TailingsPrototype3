using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkReceiver : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////
    // Message receiver class; this is meant to be changed as needed.
    ////////////////////////////////////////////////////////////////////////

    // Make sure to point this to your Network Manager instance
    public NetworkManager netManager;

    // The ID to listen for (the ID of the incoming position data)
    // and its corresponding transforms. I think it would be best to
    // wrap this in a way where you instantiate new elements as you
    // detect new ID's, but this hard-coded way should be clear enough
    // to modify.
    public int listenForThisId;
    public EcgMessage ecgMessage;

    // We'll get these from the Network Manager at runtime:
    private bool showDebug;
    private int uniqueId;

    private void Start() {
        // Set these vars from the Network Manager:
        showDebug = netManager.showDebug;
        uniqueId = netManager.uniqueId;

        initStructs();
     }

    // This is called from the network manager
    public void UpdateData(EcgMessageRaw msg) {
        ecgMessage.player_index = msg.player_index;

        NetworkUtil.setMinMax(ref ecgMessage.ecg.placeHolder1, msg.ecg[0]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.ecgRaw, msg.ecg[1]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.ecgCooked, msg.ecg[2]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.bpm, msg.ecg[3]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.r2r, msg.ecg[4]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.resp, msg.ecg[5]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.respRate, msg.ecg[6]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.placeHolder8, msg.ecg[7]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.placeHolder9, msg.ecg[8]);
        NetworkUtil.setMinMax(ref ecgMessage.ecg.placeHolder10, msg.ecg[9]);

        NetworkUtil.setMinMax(ref ecgMessage.art_chem.delight, msg.art_chem[0]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.desire, msg.art_chem[1]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.sadness, msg.art_chem[2]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.fear, msg.art_chem[3]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.ambivalence, msg.art_chem[4]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.aggressiveness, msg.art_chem[5]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.friendliness, msg.art_chem[6]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.excitement, msg.art_chem[7]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.cowardice, msg.art_chem[8]);
        NetworkUtil.setMinMax(ref ecgMessage.art_chem.melancholy, msg.art_chem[9]);
    }

    private void initStructs() {
        ecgMessage = new EcgMessage();
        ecgMessage.player_index = 0;

        ecgMessage.ecg = new EcgData();
        ecgMessage.ecg.placeHolder1 = Vector3.zero;
        ecgMessage.ecg.ecgRaw = Vector3.zero;
        ecgMessage.ecg.ecgCooked = Vector3.zero;
        ecgMessage.ecg.bpm = Vector3.zero;
        ecgMessage.ecg.r2r = Vector3.zero;
        ecgMessage.ecg.resp = Vector3.zero;
        ecgMessage.ecg.respRate = Vector3.zero;
        ecgMessage.ecg.placeHolder8 = Vector3.zero;
        ecgMessage.ecg.placeHolder9 = Vector3.zero;
        ecgMessage.ecg.placeHolder10 = Vector3.zero;

        ecgMessage.art_chem = new EmotionData();
        ecgMessage.art_chem.delight = Vector3.zero;
        ecgMessage.art_chem.desire = Vector3.zero;
        ecgMessage.art_chem.sadness = Vector3.zero;
        ecgMessage.art_chem.fear = Vector3.zero;
        ecgMessage.art_chem.ambivalence = Vector3.zero;
        ecgMessage.art_chem.aggressiveness = Vector3.zero;
        ecgMessage.art_chem.friendliness = Vector3.zero;
        ecgMessage.art_chem.excitement = Vector3.zero;
        ecgMessage.art_chem.cowardice = Vector3.zero;
        ecgMessage.art_chem.melancholy = Vector3.zero;
    }

}
