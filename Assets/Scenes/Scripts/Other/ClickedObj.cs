using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ClickedObj
{
    public void Click(PointerEventData.InputButton ibtn);
}