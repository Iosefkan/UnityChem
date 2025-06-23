using CalenderDatabase;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class Extensions
{
    public static Vector3 Clamp(this Vector3 value, float min, float max)
    {
        var x = Mathf.Clamp(value.x, min, max);
        var y = Mathf.Clamp(value.y, min, max);
        var z = Mathf.Clamp(value.z, min, max);
        return new Vector3(x, y, z);
    }

    public static float MaxX(this Vector3[][] value)
    {
        return value.Select(vec => vec.Max(p => p.x)).Max();
    }

    public static float MaxY(this Vector3[][] value)
    {
        return value.Select(vec => vec.Max(p => p.y)).Max();
    }

    public static float MaxZ(this Vector3[][] value)
    {
        return value.Select(vec => vec.Max(p => p.z)).Max();
    }

    public static float MinX(this Vector3[][] value)
    {
        return value.Select(vec => vec.Min(p => p.x)).Min();
    }

    public static float MinY(this Vector3[][] value)
    {
        return value.Select(vec => vec.Min(p => p.y)).Min();
    }

    public static float MinZ(this Vector3[][] value)
    {
        return value.Select(vec => vec.Min(p => p.z)).Min();
    }

    public static void Refresh(this TMP_Dropdown dropdown)
    {
        dropdown.enabled = false;
        dropdown.enabled = true;
        dropdown.Show();
    }
}
