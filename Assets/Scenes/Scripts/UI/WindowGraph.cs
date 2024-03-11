using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    public string graphName = string.Empty;
    public string xName = string.Empty;
    public string yName = string.Empty;

    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateHor;
    private RectTransform dashTemplateVer;
    private List<GameObject> gameObjectList;

    private void Awake()
    {
        gameObjectList = new List<GameObject>();

        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateHor = graphContainer.Find("dashTemplateHor").GetComponent<RectTransform>();
        dashTemplateVer = graphContainer.Find("dashTemplateVer").GetComponent<RectTransform>();
        if (graphName != string.Empty)
        {
            transform.Find("graphName").GetComponent<Text>().text = graphName;
        }
    }

    public void Show(List<System.Numerics.Vector2> valueList)
    {
        foreach (GameObject gameObject in gameObjectList) 
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        if (valueList.Count == 0) return;
        
        float graphWidth = graphContainer.rect.width;
        float graphHeight = graphContainer.rect.height;

        float yMaximum = valueList[0].X;
        float yMinimum = valueList[0].Y;
        float xMaximum = valueList[0].X;
        float xMinimum = valueList[0].X;

        for (int i = 0; i < valueList.Count; i++)
        {
            float value = valueList[i].Y;
            if (value > yMaximum)       yMaximum = value; 
            else if (value < yMinimum)  yMinimum = value;

            value = valueList[i].X;
            if (value > xMaximum)       xMaximum = value;
            else if (value < xMinimum)  xMinimum = value;
        }

        //float yDifference = yMaximum - yMinimum;
        //if (yDifference <= 0)
        //{
        //    yDifference = 5f;
        //}
        //yMaximum = yMaximum + (yDifference * 0.2f);
        //yMinimum = yMinimum - (yDifference * 0.2f);

        //yMinimum = 0f; // Start the graph at zero

        //float xDifference = xMaximum - xMinimum;
        //if (xDifference <= 0)
        //{
        //    xDifference = 5f;
        //}
        //xMaximum = xMaximum + (xDifference * 0.2f);
        //yMinimum = yMinimum - (yDifference * 0.2f);

        //xMinimum = 0f; // Start the graph at zero

        int xIndex = 0;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = ((valueList[i].X - xMinimum) / (xMaximum - xMinimum)) * graphWidth;
            float yPosition = ((valueList[i].Y - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            gameObjectList.Add(circleGameObject);
            if (lastCircleGameObject != null) 
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, 
                                                                            circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastCircleGameObject = circleGameObject;

            //RectTransform labelX = Instantiate(labelTemplateX);
            //labelX.SetParent(graphContainer, false);
            //labelX.gameObject.SetActive(true);
            //labelX.anchoredPosition = new Vector2(xPosition, -7f);
            //labelX.GetComponent<Text>().text = getAxisLabelX(i);
            //gameObjectList.Add(labelX.gameObject);

            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer, false);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, -3f); 
            //gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) 
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-10f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = string.Format("{0:f2}", (yMinimum + (normalizedValue * (yMaximum - yMinimum))));
            gameObjectList.Add(labelY.gameObject);

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            normalizedValue = i * 1f / separatorCount;
            labelX.anchoredPosition = new Vector2(normalizedValue * graphWidth, -15f);
            labelX.GetComponent<Text>().text = string.Format("{0:f2}",(xMinimum + (normalizedValue * (xMaximum - xMinimum))));
            gameObjectList.Add(labelX.gameObject);

            if (i == separatorCount)
            {
                if (yName != string.Empty) labelY.GetComponent<Text>().text = yName;
                if (xName != string.Empty) labelX.GetComponent<Text>().text = xName;
            }

            if (i == 0) continue;

            RectTransform dashHor = Instantiate(dashTemplateHor);
            dashHor.SetParent(graphContainer, false);
            dashHor.gameObject.SetActive(true);
            dashHor.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            gameObjectList.Add(dashHor.gameObject);

            RectTransform dashVer = Instantiate(dashTemplateVer);
            dashVer.SetParent(graphContainer, false);
            dashVer.gameObject.SetActive(true);
            dashVer.anchoredPosition = new Vector2(normalizedValue * graphWidth, -4f);
            gameObjectList.Add(dashVer.gameObject);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = Color.black;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        return gameObject;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
