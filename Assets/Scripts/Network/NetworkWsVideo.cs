using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWsVideo : MonoBehaviour {

    public NetworkManager netManager;
    public Renderer ren;
    public float fps = 30f;

    private Texture2D tex;
    private float fpsInterval;
	private bool blocked = false;

    private void Start() {
        tex = new Texture2D(1, 1);
        fpsInterval = 1f / fps;
    }

	private IEnumerator UpdateTexture(VideoMessage msg) {
        if (msg.video.Length > 1) {
            tex.LoadImage(System.Convert.FromBase64String(msg.video));
            tex.Apply();
            ren.sharedMaterial.mainTexture = tex;
            ren.sharedMaterial.SetTexture("_EmissionMap", tex);
        }
        yield return new WaitForSeconds(fpsInterval);
        blocked = false;
	}

	public void UpdateData(VideoMessage msg) {
        if (!blocked) {
            blocked = true;
            StartCoroutine(UpdateTexture(msg));
        }
    }

}
