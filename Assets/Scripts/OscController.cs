using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC {

    public class OscController : MonoBehaviour {

        public int receivePort = 9000;
        public float bciVal1 = 0f;
        public float bciVal2 = 0f;
        public float bciVal3 = 0f;
        public float bciVal4 = 0f;
        public float bciVal5 = 0f;
        public float bciVal6 = 0f;

        private OSCReceiver receiver;

        private void Start() {
            receiver = gameObject.AddComponent<OSCReceiver>();
            receiver.LocalPort = receivePort;
            receiver.LocalHost = "127.0.0.1";

            receiver.Bind("/simbci", msgRx_SimBci);
        }

        protected void msgRx_SimBci(OSCMessage message) {
            bciVal1 = message.Values[0].FloatValue;
            bciVal2 = message.Values[1].FloatValue;
            bciVal3 = message.Values[2].FloatValue;
            bciVal4 = message.Values[3].FloatValue;
            bciVal5 = message.Values[4].FloatValue;
            bciVal6 = message.Values[5].FloatValue;
        }

    }

}