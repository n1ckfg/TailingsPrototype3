using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityOSC;

public class OscController : MonoBehaviour {

	[Serializable]
	public struct EcgData {
        public int active;
        public float ecgRaw;
        public float ecgCooked;
        public float bpm;
        public float r2r;
        public float resp;
        public float respRate;
	}

	[Serializable]
	public struct EmoData {
        public float delight;
        public float desire;
        public float sadness;
        public float fear;
        public float ambivalence;
        public float aggressiveness;
        public float friendliness;
        public float excitement;
        public float cowardice;
        public float melancholy;
	}

    public EcgData ecgData;
    public EmoData emoData;
    public LightRig lightRig;

    public enum OscMode { SEND, RECEIVE, SEND_RECEIVE };
    public OscMode oscMode = OscMode.RECEIVE;
    public enum MsgMode { P5, OF };
    public MsgMode msgMode = MsgMode.P5;
    public string outIP = "127.0.0.1";
    public int outPort = 9999;
    public int inPort = 9998;
    public int rxBufferSize = 1024000;//1024;

    private OSCServer myServer;
    private int bufferSize = 100; // Buffer size of the application (stores 100 messages from different servers)
    private int sleepMs = 10;

    private List<object> newData;
    private string newAddress;

    // Script initialization
    void Start() {
        // init OSC
        OSCHandler.Instance.Init();

        // Initialize OSC clients (transmitters)
        if (oscMode == OscMode.SEND || oscMode == OscMode.SEND_RECEIVE) {
            OSCHandler.Instance.CreateClient("myClient", IPAddress.Parse(outIP), outPort);
        }

        if (oscMode == OscMode.RECEIVE || oscMode == OscMode.SEND_RECEIVE) {
            // Initialize OSC servers (listeners)
            myServer = OSCHandler.Instance.CreateServer("myServer", inPort);
            // Set buffer size (bytes) of the server (default 1024)
            myServer.ReceiveBufferSize = rxBufferSize;
            // Set the sleeping time of the thread (default 10)
            myServer.SleepMilliseconds = sleepMs;
        }

        newData = new List<object>();

        initStructs();
    }

    // Reads all the messages received between the previous update and this one
    void Update() {
        if (oscMode == OscMode.RECEIVE || oscMode == OscMode.SEND_RECEIVE) {
            // Read received messages
            for (var i = 0; i < OSCHandler.Instance.packets.Count; i++) {
                // Process OSC
                receivedOSC(OSCHandler.Instance.packets[i]);
                // Remove them once they have been read.
                OSCHandler.Instance.packets.Remove(OSCHandler.Instance.packets[i]);
                i--;
            }
        }

        // Send random number to the client
        if (oscMode == OscMode.SEND || oscMode == OscMode.SEND_RECEIVE) {
            float randVal = UnityEngine.Random.Range(0f, 0.7f);
            OSCHandler.Instance.SendMessageToClient("myClient", "/1/fader1", randVal);
        }
    }

	private void initStructs() {
        ecgData = new EcgData();
        ecgData.active = 0;
        ecgData.ecgRaw = 0f;
        ecgData.ecgCooked = 0f;
        ecgData.bpm = 0f;
        ecgData.r2r = 0f;
        ecgData.resp = 0f;
        ecgData.respRate = 0f;

        emoData = new EmoData();
        emoData.delight = 0f;
        emoData.desire = 0f;
        emoData.sadness = 0f;
        emoData.fear = 0f;
        emoData.ambivalence = 0f;
        emoData.aggressiveness = 0f;
        emoData.friendliness = 0f;
        emoData.excitement = 0f;
        emoData.cowardice = 0f;
        emoData.melancholy = 0f;
    }

    // Process OSC message
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

        switch (newAddress) {
            case "/module/1/active":
                ecgData.active = (int) newData[0];
                break;
            case "/module/1/ecgRaw":
                ecgData.ecgRaw = (float) newData[0];
                break;
            case "/module/1/ecgCooked":
                ecgData.ecgCooked = (float) newData[0];
                break;
            case "/module/1/bpm":
                ecgData.bpm = (float) newData[0];
                break;
            case "/module/1/r2r":
                ecgData.r2r = (float) newData[0];
                break;
            case "/module/1/resp":
                ecgData.resp = (float) newData[0];
                break;
            case "/module/1/respRate":
                ecgData.respRate = (float) newData[0];
                break;
            case "/emotions":
                emoData.delight = (float) newData[0];
                emoData.desire = (float) newData[1];
                emoData.sadness = (float) newData[2];
                emoData.fear = (float) newData[3];
                emoData.ambivalence = (float) newData[4];
                emoData.aggressiveness = (float) newData[5];
                emoData.friendliness = (float) newData[6];
                emoData.excitement = (float) newData[7];
                emoData.cowardice = (float) newData[8];
                emoData.melancholy = (float) newData[9];
                break;
            case "/simled":
                if (lightRig.ready) {
                    List<Color> colors = new List<Color>();
                    byte[] newBytes = (byte[]) newData[0];
                    for (int i = 0; i < newBytes.Length; i += 3) {
                        Vector3 col = new Vector3(newBytes[i], newBytes[i+1], newBytes[i+2]) / (255f / lightRig.ledScale);
                        colors.Add(new Color(col.x, col.y, col.z));
                    }

                    Debug.Log("Received " + colors.Count + " colors.");
                    for (int i = 0; i < colors.Count; i++) {
                        lightRig.points[i].color = Color.Lerp(lightRig.points[i].color, colors[i], lightRig.ledLerpSpeed);
                    }
                }
                break;
        }
    }



}