using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Texture2D.GetPixelBilinear.html

public class LightTexMultiRig : MonoBehaviour {



    public RenderTexture rTex;
    public ReceiveMessage oscController;
    public MeshFilter[] meshFilter;
    public GameObject groupPrefab;
    public LightPoint[] points;
    public List<LightGroup> groups;
    public float ledUpdateInterval = 1f;
    public float ledLerpSpeed = 0.5f;
    public float ledScale = 1f;
    public int pointBatch = 10;
    public int sampleSkip = 1;
    public bool ready = false;
    public Color defaultColor;

    private Texture2D tex;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Rect rectTex;

    private IEnumerator Start() {
        createTexFromRtex();

        ledScale = Mathf.Clamp(ledScale, 1f, 255f);

        groups = new List<LightGroup>();

        // iterate over all meshes and make a list of all vertices and uvs.
        vertices = meshFilter[0].mesh.vertices;
        uvs = meshFilter[0].mesh.uv;
        for (int i=1; i<meshFilter.Length; i++) {
            vertices = vertices.Concat(meshFilter[i].mesh.vertices).ToArray();
            uvs = uvs.Concat(meshFilter[i].mesh.uv).ToArray();
        }

        points = new LightPoint[vertices.Length];
        Debug.Log("Light rig target mesh has " + vertices.Length + " vertices.");

        for (int i = 0; i < vertices.Length; i += pointBatch) {
            int lastPoint = pointBatch;
            if (vertices.Length - i < pointBatch) lastPoint = vertices.Length - i;
            int[] newIndices = new int[lastPoint];

            for (int j = 0; j < lastPoint; j++) {
                int loc = i + j;
                newIndices[j] = loc;
                points[loc] = new LightPoint(vertices[loc], uvs[loc], defaultColor, 1f);
            }

            LightGroup newGroup = GameObject.Instantiate(groupPrefab).GetComponent<LightGroup>();
            newGroup.transform.parent = transform;
            newGroup.init(newIndices);
            setGroupPosition(newGroup);
            groups.Add(newGroup);
        }
        
        ready = true;

        StartCoroutine(updateValues());

        yield return null;
    }

    private IEnumerator updateValues() {
        while (true) {
            getLightsFromTexture();

            for (int i = 0; i < groups.Count; i++) {
                setGroupColor(groups[i]);
                setGroupBrightness(groups[i]);
            }

            yield return new WaitForSeconds(ledUpdateInterval);
        }
    }

    public void setGroupPosition(LightGroup group) {
        int loc = group.indices.Length / 2;
        group.transform.localPosition = points[group.indices[loc]].position;       
    }

    public void setGroupColor(LightGroup group) {
        Color avgColor = Color.black;
        for (int i = 0; i < group.indices.Length; i += sampleSkip) {
            avgColor += points[group.indices[i]].color;
        }
        
        group.avgColor = avgColor /= group.indices.Length;
    }

    public void setGroupBrightness(LightGroup group) {
        float avgBrightness = 0f;
        for (int i = 0; i < group.indices.Length; i += sampleSkip) {
            avgBrightness += points[group.indices[i]].brightness;
        }

        group.avgBrightness = avgBrightness / group.indices.Length;
    }

    public void getLightsFromTexture() {
        if (ready) {
            updateTexFromRtex();
            for (int i = 0; i < points.Length; i += sampleSkip) {
                Color col = tex.GetPixelBilinear(points[i].uv.x, points[i].uv.y) * ledScale;
                points[i].color = col; // Color.Lerp(points[i].color, col, ledLerpSpeed);
            }
        }
    }

    public void createTexFromRtex() {
        rectTex = new Rect(0, 0, rTex.width, rTex.height);
        tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        updateTexFromRtex();
    }

    public void updateTexFromRtex() {
        RenderTexture.active = rTex;
        tex.ReadPixels(rectTex, 0, 0);
        tex.Apply();
        RenderTexture.active = null;
    }

 }
