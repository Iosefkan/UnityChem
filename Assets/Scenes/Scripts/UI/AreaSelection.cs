using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AreaSelection : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Image displayAvgColor;
    [SerializeField] private float borderThicknessFactor = 0.01f;
    [SerializeField] private SetCieLabText text;

    private bool firstDraw = true;
    private Texture2D origTex;
    private Vector2 start, current;
    private Color average;
    private RectTransform rect;

    public void Start()
    {
        rect = image.rectTransform;
    }

    public void OnPointerDown(PointerEventData e)
    {
        try
        {
            _ = image.sprite;
            _ = image.sprite.texture;
        } 
        catch
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, e.pointerCurrentRaycast.screenPosition, e.pressEventCamera, out start))
        {
            return;
        }

        Rect rec = rect.rect;
        float normalizedX = (start.x - rec.xMin) / rec.width;
        float normalizedY = (start.y - rec.yMin) / rec.height;

        Texture2D texture = image.sprite.texture;
        int textureX = Mathf.Clamp((int)(normalizedX * texture.width), 0, texture.width - 1);
        int textureY = Mathf.Clamp((int)(normalizedY * texture.height), 0, texture.height - 1);
        start = new Vector2(textureX, textureY);
        Debug.Log("Clicked: " + start);
    }
    public void OnBeginDrag(PointerEventData e)
    {
        try
        {
            _ = image.sprite;
            _ = image.sprite.texture;
        }
        catch
        {
            return;
        }

        if (firstDraw)
        {
            origTex = CopyTexture(image.sprite.texture);
            firstDraw = false;
        }
        else
        {
            image.sprite = Sprite.Create(origTex, new Rect(0, 0, origTex.width, origTex.height), new Vector2(0.5f, 0.5f), 100f);
        }
    }

    public void OnDrag(PointerEventData e)
    {
        try
        {
            _ = image.sprite;
            _ = image.sprite.texture;
        }
        catch
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, e.pointerCurrentRaycast.screenPosition, e.pressEventCamera, out current))
        {
            return;
        }

        Rect rec = rect.rect;
        float normalizedX = (current.x - rec.xMin) / rec.width;
        float normalizedY = (current.y - rec.yMin) / rec.height;

        Texture2D texture = image.sprite.texture;
        int textureX = Mathf.Clamp((int)(normalizedX * texture.width), 0, texture.width - 1);
        int textureY = Mathf.Clamp((int)(normalizedY * texture.height), 0, texture.height - 1);
        current = new Vector2(textureX, textureY);

        DrawRectangle(start, current, Color.red);
    }

    public void OnEndDrag(PointerEventData e)
    {
        try
        {
            _ = image.sprite;
            _ = image.sprite.texture;
        }
        catch
        {
            return;
        }

        average = GetAverageColor(start, current);
        displayAvgColor.color = average;
        text.SetColor(average);
        var labColor = ColorHelper.RGBToLab(average);
        Debug.Log("Average color: " + (Color32)average);
        Debug.Log("CieLab color: " + labColor);
        Debug.Log("Converted RGB color: " + (Color32)ColorHelper.LabToRGB(labColor));
    }

    public void DrawRectangle(Vector2 pointA, Vector2 pointB, Color color)
    {
        Texture2D texture = CopyTexture(origTex);
        int startX = Mathf.Clamp((int)Mathf.Min(pointA.x, pointB.x), 0, texture.width);
        int startY = Mathf.Clamp((int)Mathf.Min(pointA.y, pointB.y), 0, texture.height);
        int endX = Mathf.Clamp((int)Mathf.Max(pointA.x, pointB.x), 0, texture.width - 1);
        int endY = Mathf.Clamp((int)Mathf.Max(pointA.y, pointB.y), 0, texture.height - 1);

        int borderThickness = Mathf.Max(1, (int)(borderThicknessFactor * Mathf.Min(texture.width, texture.height)));

        for (int y = 0; y < borderThickness; y++)
        {
            for (int x = startX; x <= endX; x++)
            {
                if (startY + y < texture.height) texture.SetPixel(x, startY + y, color);
                if (endY - y >= 0) texture.SetPixel(x, endY - y, color);
            }
        }

        for (int x = 0; x < borderThickness; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (startX + x < texture.width) texture.SetPixel(startX + x, y, color);
                if (endX - x >= 0) texture.SetPixel(endX - x, y, color);
            }
        }

        texture.Apply();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
    }

    public Color GetAverageColor(Vector2 pointA, Vector2 pointB)
    {
        Texture2D texture = CopyTexture(origTex);
        int startX = Mathf.Clamp((int)Mathf.Min(pointA.x, pointB.x), 0, texture.width);
        int startY = Mathf.Clamp((int)Mathf.Min(pointA.y, pointB.y), 0, texture.height);
        int endX = Mathf.Clamp((int)Mathf.Max(pointA.x, pointB.x), 0, texture.width - 1);
        int endY = Mathf.Clamp((int)Mathf.Max(pointA.y, pointB.y), 0, texture.height - 1);

        float r = 0;
        float g = 0;
        float b = 0;
        int count = 0;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                var pixel = texture.GetPixel(x, y);
                r += pixel.r;
                g += pixel.g;
                b += pixel.b;
                count++;
            }
        }
        return new(r / count, g / count, b / count, 1);
    }

    public static Texture2D CopyTexture(Texture2D texture)
    {
        Texture2D textureClone = new Texture2D(texture.width, texture.height, texture.format, false);
        textureClone.LoadRawTextureData(texture.GetRawTextureData());
        textureClone.Apply();
        return textureClone;
    }

    public void ResetOriginalTexture()
    {
        firstDraw = true;
    }
}
