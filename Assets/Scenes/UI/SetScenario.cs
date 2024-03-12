using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static EntryValuePanel;

public class SetScenario : MonoBehaviour
{
    const int MAX_COUNT = 100;

    ScrollView currPanel = null;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        if (root == null) return;
        int i = 0;
        do
        {
            Button btn = root.Q<Button>($"btn{i + 1}");
            ScrollView v = root.Q<ScrollView>($"sw{i + 1}");

            if (btn == null || v == null) break;

            if (i == 0) ChangeTab(v);
            else        v.style.display = DisplayStyle.None;
            
            btn.clicked += () => ChangeTab(v);
        }
        while (++i < MAX_COUNT);
    }

    void ChangeTab(ScrollView sv)
    {
        if (currPanel != null) currPanel.style.display = DisplayStyle.None;
        currPanel = sv;
        currPanel.style.display = DisplayStyle.Flex;
    }
}
