using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{

    public float range = 0.2f;
    public AudioSource audio;

    private void Start() {
        float newPitch = 1f + Random.Range(-range, range);
        audio.pitch = newPitch;
    }

}
