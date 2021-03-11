using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoObj : MonoBehaviour {

    public float rot = 1f;
    public float changeDirInterval = 3f;

    private Vector3 rotVec = Vector3.zero;

    private void Start() {
        StartCoroutine(changeDirection());
    }

    private void Update() {
        transform.Rotate(rotVec);
    }

    private IEnumerator changeDirection() {
        while (true) {
            float nextDirInterval = Random.Range(changeDirInterval / 2f, changeDirInterval * 2f);
            float nextRot = Random.Range(rot / 2f, rot * 2f);

            int rotateDir = (int) Random.Range(0f, 3f);

            switch (rotateDir) {
                case 0:
                    rotVec = new Vector3(nextRot, 0f, 0f);
                    break;
                case 1:
                    rotVec = new Vector3(0f, nextRot, 0f);
                    break;
                case 2:
                    rotVec = new Vector3(0f, 0f, nextRot);
                    break;
            }

            yield return new WaitForSeconds(nextDirInterval);
        }
    }

}
