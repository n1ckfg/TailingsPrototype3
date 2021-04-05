using UnityEngine;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////
// This file defines the serializable data types that we're sending
// back and forth to the server. Unity can convert a json into one of 
// these structs.
////////////////////////////////////////////////////////////////////////

[System.Serializable]
public struct EcgMessageRaw {
	public int player_index;
    public float[] ecg;
    public float[] art_chem;
}

[System.Serializable]
public struct EcgMessage {
    public int player_index;
    public EcgData ecg;
    public EmotionData art_chem;
}

[System.Serializable]
public struct EcgData {
    public Vector3 placeHolder1;
    public Vector3 ecgRaw; // 2
    public Vector3 ecgCooked; // 3
    public Vector3 bpm; // 4
    public Vector3 r2r; // 5
    public Vector3 resp; // 6
    public Vector3 respRate; // 7
    public Vector3 placeHolder8;
    public Vector3 placeHolder9;
    public Vector3 placeHolder10;
    public Vector3 placeHolder11;
    public Vector3 placeHolder12;
}

[System.Serializable]
public struct EmotionData {
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

[System.Serializable]
public struct VideoMessage {
    public string unique_id;
    public string video;
    public string timestamp;
}