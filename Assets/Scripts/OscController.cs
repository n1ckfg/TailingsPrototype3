using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC {

    public class OscController : MonoBehaviour {

        public LightRig lightRig;
        public int receivePort = 9000;
        public float[] bciVal;

        private OSCReceiver receiver;

        private void Start() {
            receiver = gameObject.AddComponent<OSCReceiver>();
            receiver.LocalPort = receivePort;

            receiver.Bind("/simbci", msgRx_Bci);
            receiver.Bind("/simled", msgRx_Led);

            bciVal = new float[6];
        }

        protected void msgRx_Bci(OSCMessage message) {
            bciVal[0] = message.Values[0].FloatValue;
            bciVal[1] = message.Values[1].FloatValue;
            bciVal[2] = message.Values[2].FloatValue;
            bciVal[3] = message.Values[3].FloatValue;
            bciVal[4] = message.Values[4].FloatValue;
            bciVal[5] = message.Values[5].FloatValue;
        }

        protected void msgRx_Led(OSCMessage message) {
            if (lightRig.ready) {
                List<Color> colors = OscUtil.bytesToColors(message.Values[0].BlobValue);
                Debug.Log("Received " + colors.Count + " colors.");
                for (int i = 0; i < colors.Count; i++) {
                    lightRig.points[i].color = colors[i];
                }
            }
        }

    }

}