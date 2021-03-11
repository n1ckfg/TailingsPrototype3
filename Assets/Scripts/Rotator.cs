using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public Vector3 rot = Vector3.zero;

    private void Start() {
        rot.x = 1f;
    }

    private void Update() {
        transform.Rotate(rot);
    }

}
