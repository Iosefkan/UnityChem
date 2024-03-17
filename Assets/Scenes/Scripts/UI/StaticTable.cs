//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//[ExecuteInEditMode]
//public class StaticTable : MonoBehaviour
//{
//    [Serializable]
//    class ChangedValue
//    {
//        private float val = 0;
        
//        public 
//    }

//    [SerializeField] private float defaultRowSize = 10;
//    [SerializeField] private float defaultColumnSize = 10;
//    [SerializeField] private List<float> rowsSize = new List<float>();
//    [SerializeField] private List<float> columnsSize = new List<float>();
//    private int currRowCount = 0;
//    private int currColumnCount = 0;

//    private List<List<GameObject>> objects = new List<List<GameObject>>();
    
//    void Awake()
//    {
//        var s = gameObject.AddComponent<ContentSizeFitter>();
//        s.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
//        s.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


//        gameObject.AddComponent<VerticalLayoutGroup>();
//    }

//    private void Update()
//    {
//        if (currRowCount != rowsSize.Count)
//        {
//            UpdateRows();
//        }
//        if (currColumnCount != columnsSize.Count)
//        {
//            UpdateColumns();
//        }

//        if ()
//    }

//    void UpdateRows()
//    {
//        if (currRowCount < rowsSize.Count)
//        {
//            for (int i = currRowCount; i < rowsSize.Count; ++i)
//            {
//                rowsSize[i] = defaultRowSize;
//                objects.Add(new List<GameObject>());
//                foreach (var cellWhidth in columnsSize)
//                {
//                    objects.Last().Add(NewCell(cellWhidth, rowsSize[i]));
//                }
//            }
//        }
//        else if (currRowCount > rowsSize.Count)
//        {
//            for (int i = currRowCount; i > rowsSize.Count; --i)
//            {
//                objects.RemoveAt(i - 1);
//            }
//        }

//        currRowCount = rowsSize.Count;
//    }

//    void UpdateColumns()
//    {
//        if (currColumnCount < columnsSize.Count)
//        {
//            for (int j = currColumnCount; j < columnsSize.Count; ++j)
//            {
//                columnsSize[j] = defaultColumnSize;
//                for (int i = 0; i < rowsSize.Count; ++i)
//                {

//                    objects[i].Add(NewCell(columnsSize[j], rowsSize[i]));
//                }
//            }
//        }
//        else if (currColumnCount > columnsSize.Count)
//        {
//            for (int i = 0; i < rowsSize.Count; ++i)
//            {
//                for (int j = currColumnCount; j > columnsSize.Count; --j)
//                {
//                    objects[i].RemoveAt(j - 1);
//                }
//            }
//        }

//        currColumnCount = columnsSize.Count;
//    }

//    GameObject NewCell(float w, float h)
//    {
//        GameObject cell = new GameObject();
//        cell.AddComponent<CanvasRenderer>();
//        cell.transform.parent = transform;
//        RectTransform rect = cell.AddComponent<RectTransform>();
//        rect.localPosition = Vector3.zero;
//        rect.localRotation = Quaternion.identity;
//        rect.localScale = Vector3.one;
//        rect.sizeDelta = new Vector2(w, h);
//        return cell;
//    }
//}
