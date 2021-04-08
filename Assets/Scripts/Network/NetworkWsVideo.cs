using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWsVideo : MonoBehaviour {

    public NetworkManager netManager;
    public RenderHeads.Media.AVProVideo.MediaPlayer video;
    public RenderHeads.Media.AVProVideo.ApplyToMaterial videoMaterialControl;
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
            videoMaterialControl.enabled = bypass;
            if (bypass) {
                video.Play();
            } else {
                video.Stop();
            }
        }
    }

	private IEnumerator UpdateTexture(VideoMessage msg) {
        if (!bypass && msg.video.Length > 1) {
            tex.LoadImage(System.Convert.FromBase64String(msg.video));
            tex.Apply();
            ren.sharedMaterial.mainTexture = tex;
            //ren.sharedMaterial.SetTexture("_EmissionMap", tex);
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
