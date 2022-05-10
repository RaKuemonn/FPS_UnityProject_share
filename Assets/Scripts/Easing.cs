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

    public static Vector2 SineInOutV2(float t, float totaltime, Vector2 min, Vector2 max)
    {
        max -= min;
        return -max / 2 * (Mathf.Cos(t * Mathf.PI / totaltime) - 1) + min;
    }
}

public class MathHelpar
{
    public static Vector2 AngleToVector2(float angle)
    {
        var radian = angle * (Mathf.PI / 180);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
    }
};