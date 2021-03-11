using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Texture2D.GetPixelBilinear.html

public class LightTexRig : MonoBehaviour {

    public OscController oscController;
    public MeshFilter meshFilter;
    public GameObject groupPrefab;
    public LightPoint[] points;
    public List<LightGroup> groups;
    public float ledUpdateInterval = 1f;
    public float ledLerpSpeed = 0.5f;
    public float ledScale = 1f;
    public int pointBatch = 10;
    public bool ready = false;
    public Color defaultColor;

    private Vector3[] vertices;
    private Color[] cols;

    private IEnumerator Start() {
        ledScale = Mathf.Clamp(ledScale, 1f, 255f);

        groups = new List<LightGroup>();

        vertices = meshFilter.mesh.vertices;
        Debug.Log("Light rig target mesh has " + vertices.Length + " vertices.");
        points = new LightPoint[vertices.Length];

        cols = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++) {
            cols[i] = defaultColor;
        }

        meshFilter.mesh.colors = cols;

        for (int i = 0; i < vertices.Length; i += pointBatch) {
            int lastPoint = pointBatch;
            if (vertices.Length - i < pointBatch) lastPoint = vertices.Length - i;
            int[] newIndices = new int[lastPoint];

            for (int j = 0; j < lastPoint; j++) {
                int loc = i + j;
                newIndices[j] = loc;
                points[loc] = new LightPoint(vertices[loc], defaultColor, 1f);
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
            getLightsFromOsc();

            for (int i = 0; i < groups.Count; i++) {
                setGroupColor(groups[i]);
                setGroupBrightness(groups[i]);
            }

            setAllCols();

            yield return new WaitForSeconds(ledUpdateInterval);
        }
    }

    public void setGroupPosition(LightGroup group) {
        int loc = group.indices.Length / 2;
        group.transform.localPosition = points[group.indices[loc]].position;       
    }

    public void setGroupColor(LightGroup group) {
        Color avgColor = Color.black;
        for (int i = 0; i < group.indices.Length; i++) {
            avgColor += points[group.indices[i]].color;
        }
        
        group.avgColor = avgColor /= group.indices.Length;
    }

    public void setGroupBrightness(LightGroup group) {
        float avgBrightness = 0f;
        for (int i = 0; i < group.indices.Length; i++) {
            avgBrightness += points[group.indices[i]].brightness;
        }

        group.avgBrightness = avgBrightness / group.indices.Length;
    }
    
    public void setAllCols() {
        for (int i=0; i<points.Length; i++) {
            cols[i] = points[i].color;
        }

        meshFilter.mesh.colors = cols;       
    }

    public void getLightsFromOsc() {
        if (ready) {
            List<Color> colors = new List<Color>();
            byte[] newBytes = oscController.lightBytes;
            for (int i = 0; i < newBytes.Length; i += 3) {
                Vector3 col = new Vector3(newBytes[i], newBytes[i + 1], newBytes[i + 2]) / (255f / ledScale);
                colors.Add(new Color(col.x, col.y, col.z));
            }

            Debug.Log("Received " + colors.Count + " colors.");
            for (int i = 0; i < colors.Count; i++) {
                points[i].color = Color.Lerp(points[i].color, colors[i], ledLerpSpeed);
            }
        }
    }
}
