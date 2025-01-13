using UnityEngine;
using UnityEngine.UI;

public class GetColor : MonoBehaviour
{
    public Color Get()
    {
        return gameObject.GetComponent<Image>().color;
    }
}
