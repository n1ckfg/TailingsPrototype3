using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using UnityOSC;

public class OscController : MonoBehaviour {

    [Serializable]
    public struct EcgData {
        public int active;
        public Vector3 ecgRaw;
        public Vector3 ecgCooked;
        public Vector3 bpm;
        public Vector3 r2r;
        public Vector3 resp;
        public Vector3 respRate;
    }

    [Serializable]
    public struct EmoData {
        public Vector3 delight;
        public Vector3 desire;
        public Vector3 sadness;
        public Vector3 fear;
        public Vector3 ambivalence;
        public Vector3 aggressiveness;
        public Vector3 friendliness;
        public Vector3 excitement;
        public Vector3 cowardice;
        public Vector3 melancholy;
    }

    [HideInInspector] public byte[] lightBytes;
    public EcgData ecgData;
    public EmoData emoData;

    public enum OscMode { SEND, RECEIVE, SEND_RECEIVE };
    public OscMode oscMode = OscMode.RECEIVE;
    public enum MsgMode { P5, OF };
    public MsgMode msgMode = MsgMode.P5;
    public string outIP = "127.0.0.1";
    public int outPort = 9999;
    public int inPort = 9998;
    public int rxBufferSize = 1024000;//1024;

    //private OSCServer myServer;
    private int bufferSize = 100; // Buffer size of the application (stores 100 messages from different servers)
    private int sleepMs = 10;

    private List<object> newData;
    private string newAddress;

    // Script initialization
    void Start() {
        // init OSC
       // OSCHandler.Instance.Init();

        // Initialize OSC clients (transmitters)
        if (oscMode == OscMode.SEND || oscMode == OscMode.SEND_RECEIVE) {
            //OSCHandler.Instance.CreateClient("myClient", IPAddress.Parse(outIP), outPort);
        }

        if (oscMode == OscMode.RECEIVE || oscMode == OscMode.SEND_RECEIVE) {
            // Initialize OSC servers (listeners)
            //myServer = OSCHandler.Instance.CreateServer("myServer", inPort);
            // Set buffer size (bytes) of the server (default 1024)
            //myServer.ReceiveBufferSize = rxBufferSize;
            // Set the sleeping time of the thread (default 10)
            //myServer.SleepMilliseconds = sleepMs;
        }

        newData = new List<object>();

        initStructs();
    }

    // Reads all the messages received between the previous update and this one
    void Update() {
        if (oscMode == OscMode.RECEIVE || oscMode == OscMode.SEND_RECEIVE) {
            // Read received messages
            //for (var i = 0; i < OSCHandler.Instance.packets.Count; i++) {
                // Process OSC
                //receivedOSC(OSCHandler.Instance.packets[i]);
                // Remove them once they have been read.
                //OSCHandler.Instance.packets.Remove(OSCHandler.Instance.packets[i]);
                //i--;
            //}
        }

        // Send random number to the client
        if (oscMode == OscMode.SEND || oscMode == OscMode.SEND_RECEIVE) {
            float randVal = UnityEngine.Random.Range(0f, 0.7f);
            //OSCHandler.Instance.SendMessageToClient("myClient", "/1/fader1", randVal);
        }
    }

    private void initStructs() {
        ecgData = new EcgData();
        ecgData.active = 0;
        ecgData.ecgRaw = Vector3.zero;
        ecgData.ecgCooked = Vector3.zero;
        ecgData.bpm = Vector3.zero;
        ecgData.r2r = Vector3.zero;
        ecgData.resp = Vector3.zero;
        ecgData.respRate = Vector3.zero;

        emoData = new EmoData();
        emoData.delight = Vector3.zero;
        emoData.desire = Vector3.zero;
        emoData.sadness = Vector3.zero;
        emoData.fear = Vector3.zero;
        emoData.ambivalence = Vector3.zero;
        emoData.aggressiveness = Vector3.zero;
        emoData.friendliness = Vector3.zero;
        emoData.excitement = Vector3.zero;
        emoData.cowardice = Vector3.zero;
        emoData.melancholy = Vector3.zero;

        //StartCoroutine(checkMinMax());
    }

    private IEnumerator checkMinMax() {
        while (true) {
            Debug.Log(printMinMax("ecgRaw", ecgData.ecgRaw) + printMinMax("ecgCooked", ecgData.ecgCooked) + printMinMax("bpm", ecgData.bpm) + printMinMax("r2r", ecgData.r2r) + printMinMax("resp", ecgData.resp) + printMinMax("respRate", ecgData.respRate));

            yield return new WaitForSeconds(3f);
        }
    }

    string printMinMax(string name, Vector3 input) {
        return " " + name + " (" + input.y + ", " + input.z + ")";
    }

    // Process OSC message
    /*
    private void receivedOSC(OSCPacket pckt) {
        if (pckt == null) {
            Debug.Log("Empty packet");
            return;
        }

        switch (msgMode) {
            case (MsgMode.P5):
                newData = pckt.Data;
                newAddress = pckt.Address;
                break;
            case (MsgMode.OF):
                OSCMessage msg = pckt.Data[0] as UnityOSC.OSCMessage;
                newData = msg.Data;
                newAddress = msg.Address;
                break;
        }

        //Debug.Log(newData);

        switch (newAddress) {
            case "/module/1/active":
                ecgData.active = (int)newData[0];
                break;
            case "/module/1/ecgRaw":
                setMinMax(ref ecgData.ecgRaw, (float)newData[0]);
                break;
            case "/module/1/ecgCooked":
                setMinMax(ref ecgData.ecgCooked, (float)newData[0]);
                break;
            case "/module/1/bpm":
                setMinMax(ref ecgData.bpm, (float)newData[0]);
                break;
            case "/module/1/r2r":
                setMinMax(ref ecgData.r2r, (float)newData[0]);
                break;
            case "/module/1/resp":
                setMinMax(ref ecgData.resp, (float)newData[0]);
                break;
            case "/module/1/respRate":
                setMinMax(ref ecgData.respRate, (float)newData[0]);
                break;
            case "/emotions":
                setMinMax(ref emoData.delight, (float)newData[0]);
                setMinMax(ref emoData.desire, (float)newData[1]);
                setMinMax(ref emoData.sadness, (float)newData[2]);
                setMinMax(ref emoData.fear, (float)newData[3]);
                setMinMax(ref emoData.ambivalence, (float)newData[4]);
                setMinMax(ref emoData.aggressiveness, (float)newData[5]);
                setMinMax(ref emoData.friendliness, (float)newData[6]);
                setMinMax(ref emoData.excitement, (float)newData[7]);
                setMinMax(ref emoData.cowardice, (float)newData[8]);
                setMinMax(ref emoData.melancholy, (float)newData[9]);
                break;
            case "/simled":
                lightBytes = (byte[]) newData[0];
                break;
        }
    }
    */

    void setMinMax(ref Vector3 result, float input) {
        result.x = input;
        if (result.y < input) result.y = input;
        if (result.z > input) result.z = input;
        result = result.normalized;
    }

}