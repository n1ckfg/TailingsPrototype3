using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWsVideo : MonoBehaviour {

    public NetworkManager netManager;
    public RenderHeads.Media.AVProVideo.MediaPlayer video;
    public Renderer ren;
    public float fps = 30f;
    public bool bypass = true;

    private Texture2D tex;
    private float fpsInterval;
	private bool blocked = false;

    private void Start() {
        tex = new Texture2D(1, 1);
        fpsInterval = 1f / fps;
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Tab)) {
            bypass = !bypass;
            if (bypass) {
                video.Stop();
            } else {
                video.Play();
            }
        }
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
        if (!blocked && !bypass) {
            blocked = true;
            StartCoroutine(UpdateTexture(msg));
        }
    }

}
