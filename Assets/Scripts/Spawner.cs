using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public OscController oscController;
    public GameObject prefab;
    public int numObj = 10;
    public float range = 5f;

    [HideInInspector] public FIMSpace.Basics.FBasic_FlyMovement[] flightControls;

    private void Start() {
        flightControls = new FIMSpace.Basics.FBasic_FlyMovement[numObj];

        for (int i=0; i<numObj; i++) {
            GameObject obj = GameObject.Instantiate(prefab);
            flightControls[i] = obj.GetComponent<FIMSpace.Basics.FBasic_FlyMovement>();

            obj.transform.position = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
            obj.transform.eulerAngles = new Vector3(Random.Range(-179f, 179f), Random.Range(-179f, 179f), Random.Range(-179f, 179f));
        }
    }

	private void Update() {
		for (int i=0; i<flightControls.Length; i++) {
            // How fast the model flies on its trajectory
            flightControls[i].MainSpeed = 2f;// oscController.ecgData.bpm; // default 1f; 

            // How far the object flies
            flightControls[i].RangeValue = Vector3.one; // default Vector3.one;

            // Multiplier for range value, applied to all axes
            flightControls[i].RangeMul = 10f; // oscController.ecgData.ecgCooked; // default 5f; 

            // Additional translation on y axis if you want movement to be like butterfly flight
            flightControls[i].AddYSin = 1f; // oscController.ecgData.resp;// default 1f;
            flightControls[i].AddYSinTimeSpeed = 1f;// oscController.ecgData.respRate; // default 1f;

            // How fast model should rotate to its forward movement direction
            flightControls[i].RotateForwardSpeed = 10f;// oscController.ecgData.r2r; // default 10f; 

        }
	}

}
