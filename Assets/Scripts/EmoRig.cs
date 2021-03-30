using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoRig : MonoBehaviour {

    public ReceiveMessage oscController;
    public EmoObj[] emos;
    public float[] scalers;

    private void Update() {
        emos[0].rot = oscController.ecgMessage.art_chem.delight.x * scalers[0];
        emos[1].rot = oscController.ecgMessage.art_chem.desire.x * scalers[1];
        emos[2].rot = oscController.ecgMessage.art_chem.sadness.x * scalers[2];
        emos[3].rot = oscController.ecgMessage.art_chem.fear.x * scalers[3];
        emos[4].rot = oscController.ecgMessage.art_chem.ambivalence.x * scalers[4];
        emos[5].rot = oscController.ecgMessage.art_chem.aggressiveness.x * scalers[5];
        emos[6].rot = oscController.ecgMessage.art_chem.friendliness.x * scalers[6];
        emos[7].rot = oscController.ecgMessage.art_chem.excitement.x * scalers[7];
        emos[8].rot = oscController.ecgMessage.art_chem.cowardice.x * scalers[8];
        emos[9].rot = oscController.ecgMessage.art_chem.melancholy.x * scalers[9];
    }

}
