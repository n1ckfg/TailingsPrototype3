using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityOSC;

public class OscController : MonoBehaviour {

    public LightRig lightRig;
    public enum OscMode { SEND, RECEIVE, SEND_RECEIVE };
    public OscMode oscMode = OscMode.RECEIVE;
    public enum MsgMode { P5, OF };
    public MsgMode msgMode = MsgMode.P5;
    public string outIP = "127.0.0.1";
    public int outPort = 9999;
    public int inPort = 9998;
    public int rxBufferSize = 1024000;//1024;

    public float[] bciVal;

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
        bciVal = new float[6];
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
            case "/simbci":
                bciVal[0] = (float) newData[0];
                bciVal[1] = (float) newData[1];
                bciVal[2] = (float) newData[2];
                bciVal[3] = (float) newData[3];
                bciVal[4] = (float) newData[4];
                bciVal[5] = (float) newData[5];
                break;
            case "/simled":
                if (lightRig.ready) {
                    List<Color> colors = OscUtil.bytesToColors((byte[]) newData[0]);
                    Debug.Log("Received " + colors.Count + " colors.");
                    for (int i = 0; i < colors.Count; i++) {
                        lightRig.points[i].color = colors[i];
                    }
                }
                break;
        }
    }



}