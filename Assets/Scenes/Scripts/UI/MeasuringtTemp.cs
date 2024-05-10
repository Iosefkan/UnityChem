using UnityEngine;
using UnityEngine.UI;

public class MeasuringtTemp : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] float height;
    [SerializeField] float value;

    public void SetValue(string val)
    {
        SetValue(float.Parse(val));
    }

    public void SetValue(float val)
    {
        float ratio = height / value;

        var rt = img.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, ratio * val);
    }
}
