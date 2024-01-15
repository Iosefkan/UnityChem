using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OnOffManager;

public class OnOffManager : MonoBehaviour
{
    [System.Serializable]
    public struct OnOff
    {
        public Button btnOn;
        public Button btnOff;
        public OnOffTablo[] onOffTablos;
    }

    [SerializeField] OnOff[] onOffs;

    void Start()
    {
        foreach (OnOff onOff in onOffs)
        {
            foreach (OnOffTablo onOffTablo in onOff.onOffTablos)
            {
                onOff.btnOn.onClick.AddListener(() => { onOffTablo.ÑhangeOnOff(true); });
                onOff.btnOff.onClick.AddListener(() => { onOffTablo.ÑhangeOnOff(false); });
            }
        }
    }
}