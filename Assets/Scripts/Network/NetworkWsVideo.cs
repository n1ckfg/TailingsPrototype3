using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWsVideo : MonoBehaviour {

    public NetworkManager netManager;
    public Renderer ren;

    private Texture2D tex;
    private VideoMessage msg;

    private void Start() {
        tex = new Texture2D(1, 1);
    }

	public void UpdateData(VideoMessage _msg) {
        tex.LoadImage(System.Convert.FromBase64String(_msg.video));
        tex.Apply();
        ren.sharedMaterial.mainTexture = tex;
    }

}
