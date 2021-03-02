using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC {

    public class OscController : MonoBehaviour {

        private int receivePort = 7001;
        private OSCReceiver receiver;

        private void Awake() {
            receiver = gameObject.AddComponent<OSCReceiver>();
            receiver.LocalPort = receivePort;

            receiver.Bind("/message/address", MessageReceived);
        }

        protected void MessageReceived(OSCMessage message) {
            float value = message.Values[0].FloatValue;
            Debug.Log(value);
        }

    }

}