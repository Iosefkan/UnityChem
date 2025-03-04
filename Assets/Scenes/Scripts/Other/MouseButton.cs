using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isChangeColor = false;
    private Color _color_start;
    private Image _button;
    [Serializable] public class ButtonEvent : UnityEvent { }

    public ButtonEvent click;
    public ButtonEvent hold;
    public ButtonEvent release;

    private void OnEnable()
    {
        TryGetComponent(out _button);
        if (_button && isChangeColor)
            _color_start = _button.color;
    }

    private void OnDisable()
    {
        if (_button && isChangeColor)
            _button.color = _color_start;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Start");
        click?.Invoke();
        if (_button && isChangeColor)
            _button.color = Color.gray;
        StartCoroutine(Stay());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Stop");
        release?.Invoke();
        if (_button && isChangeColor)
            _button.color = _color_start;
        StopAllCoroutines();
    }

/*    private void OnMouseDown()
    {
        Debug.Log("Start");
        click?.Invoke();
        if (_button && isChangeColor)
            _button.color = Color.gray;
        StartCoroutine(Stay());
    }*/

/*    private void OnMouseUp()
    {

    }*/

    private IEnumerator Stay()
    {
        while (true)
        {
            hold?.Invoke();
            yield return new WaitForSeconds(0.05f);
        }
    }
}
