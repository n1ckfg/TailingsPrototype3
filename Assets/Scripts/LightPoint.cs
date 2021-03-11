using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint {

    public Vector3 position;
    public Vector2 uv;
    public Color color;
    public float brightness;

    public LightPoint(Vector3 _position, Vector2 _uv, Color _color, float _brightness) {
        position = _position;
        uv = _uv;
        color = _color;
        brightness = _brightness;
    }

}
