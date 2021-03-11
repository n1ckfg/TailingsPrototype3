using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoRig : MonoBehaviour {

    public OscController oscController;
    public EmoObj[] emos;
    public float[] scalers;

    private void Update() {
        emos[0].rot = oscController.emoData.delight.x * scalers[0];
        emos[1].rot = oscController.emoData.desire.x * scalers[1];
        emos[2].rot = oscController.emoData.sadness.x * scalers[2];
        emos[3].rot = oscController.emoData.fear.x * scalers[3];
        emos[4].rot = oscController.emoData.ambivalence.x * scalers[4];
        emos[5].rot = oscController.emoData.aggressiveness.x * scalers[5];
        emos[6].rot = oscController.emoData.friendliness.x * scalers[6];
        emos[7].rot = oscController.emoData.excitement.x * scalers[7];
        emos[8].rot = oscController.emoData.cowardice.x * scalers[8];
        emos[9].rot = oscController.emoData.melancholy.x * scalers[9];
    }

}
