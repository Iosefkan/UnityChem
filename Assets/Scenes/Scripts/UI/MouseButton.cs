using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MouseButton : MonoBehaviour
{
    public bool isChangeColor = false;
    private Color _color_start;
    private Image _button;
    [Serializable] public class ButtonEvent : UnityEvent { }

    public ButtonEvent click;

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

    private void OnMouseDown()
    {
        click?.Invoke();
        if (_button && isChangeColor)
            _button.color = Color.gray;
    }
}
