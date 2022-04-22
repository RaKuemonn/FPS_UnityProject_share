using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing 
{
    public static Vector3 SineInOut(float t, float totaltime, Vector3 min, Vector3 max)
    {
        max -= min;
        return -max / 2 * (Mathf.Cos(t * Mathf.PI / totaltime) - 1) + min;
    }
}
