using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class ColorHelper
{
    // Reference white for D65 standard illuminant
    private const float REF_X = 95.047f;
    private const float REF_Y = 100.000f;
    private const float REF_Z = 108.883f;

    /// <summary>
    /// Converts an RGB color to CIE Lab color space.
    /// </summary>
    /// <param name="color">The input color in RGB space (0-1 range).</param>
    /// <returns>A Vector3 representing the Lab color (L, a, b).</returns>
    public static Vector3 RGBToLab(Color color)
    {
        // Convert RGB to XYZ
        Vector3 xyz = RGBToXYZ(color);

        // Convert XYZ to Lab
        return XYZToLab(xyz);
    }

    /// <summary>
    /// Converts a CIE Lab color to RGB color space.
    /// </summary>
    /// <param name="lab">A Vector3 representing the Lab color (L, a, b).</param>
    /// <returns>The color in RGB space (0-1 range).</returns>
    public static Color LabToRGB(Vector3 lab)
    {
        // Convert Lab to XYZ
        Vector3 xyz = LabToXYZ(lab);

        // Convert XYZ to RGB
        return XYZToRGB(xyz);
    }

    private static Vector3 RGBToXYZ(Color color)
    {
        float r = PivotRGB(color.r);
        float g = PivotRGB(color.g);
        float b = PivotRGB(color.b);

        // Convert to XYZ using the RGB to XYZ matrix
        float x = r * 0.4124564f + g * 0.3575761f + b * 0.1804375f;
        float y = r * 0.2126729f + g * 0.7151522f + b * 0.0721750f;
        float z = r * 0.0193339f + g * 0.1191920f + b * 0.9503041f;

        return new Vector3(x * 100f, y * 100f, z * 100f);
    }

    private static Vector3 XYZToLab(Vector3 xyz)
    {
        float x = PivotXYZ(xyz.x / REF_X);
        float y = PivotXYZ(xyz.y / REF_Y);
        float z = PivotXYZ(xyz.z / REF_Z);

        float l = 116f * y - 16f;
        float a = 500f * (x - y);
        float b = 200f * (y - z);

        return new Vector3(l, a, b);
    }

    private static Vector3 LabToXYZ(Vector3 lab)
    {
        float y = (lab.x + 16f) / 116f;
        float x = lab.y / 500f + y;
        float z = y - lab.z / 200f;

        x = InversePivotXYZ(x) * REF_X;
        y = InversePivotXYZ(y) * REF_Y;
        z = InversePivotXYZ(z) * REF_Z;

        return new Vector3(x, y, z);
    }

    private static Color XYZToRGB(Vector3 xyz)
    {
        // Scale XYZ values
        float x = xyz.x / 100f;
        float y = xyz.y / 100f;
        float z = xyz.z / 100f;

        // Convert to linear RGB
        float r = x * 3.2404542f - y * 1.5371385f - z * 0.4985314f;
        float g = -x * 0.9692660f + y * 1.8760108f + z * 0.0415560f;
        float b = x * 0.0556434f - y * 0.2040259f + z * 1.0572252f;

        // Apply gamma correction
        r = Mathf.Clamp01(InversePivotRGB(r));
        g = Mathf.Clamp01(InversePivotRGB(g));
        b = Mathf.Clamp01(InversePivotRGB(b));

        return new Color(r, g, b);
    }

    private static float PivotRGB(float value)
    {
        return (value > 0.04045f) ? Mathf.Pow((value + 0.055f) / 1.055f, 2.4f) : value / 12.92f;
    }

    private static float InversePivotRGB(float value)
    {
        return (value > 0.0031308f) ? 1.055f * Mathf.Pow(value, 1f / 2.4f) - 0.055f : value * 12.92f;
    }

    private static float PivotXYZ(float value)
    {
        return (value > 0.008856f) ? Mathf.Pow(value, 1f / 3f) : (7.787f * value) + (16f / 116f);
    }

    private static float InversePivotXYZ(float value)
    {
        float cube = Mathf.Pow(value, 3f);
        return (cube > 0.008856f) ? cube : (value - 16f / 116f) / 7.787f;
    }

    public static float GetLabColorsDiff(Vector3 color1, Vector3 color2)
    {
        return Mathf.Sqrt(Mathf.Pow(color2.x - color1.x, 2) + Mathf.Pow(color2.y - color1.y, 2) + Mathf.Pow(color2.z - color1.z, 2));
    }
}
