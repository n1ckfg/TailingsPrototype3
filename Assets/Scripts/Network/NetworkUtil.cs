using UnityEngine;
using System.Collections.Generic;

// Handles object/JSON conversion
public static class NetworkUtil {

    public static void setMinMax(ref Vector3 result, float input) {
        result.x = input;
        if (result.y < input) result.y = input;
        if (result.z > input) result.z = input;
        result = result.normalized;
    }

}
