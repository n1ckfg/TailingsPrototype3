using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public int numObj = 10;
    public float range = 5f;

    private void Start() {
        for (int i=0; i<numObj; i++) {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.position = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
            obj.transform.eulerAngles = new Vector3(Random.Range(-179f, 179f), Random.Range(-179f, 179f), Random.Range(-179f, 179f));
        }
    }

}
