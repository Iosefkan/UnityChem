using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class DataPlotter : MonoBehaviour
{
    private enum Axis
    {
        X, Y, Z
    }

    [Header("Data Settings")]
    public float scaleFactor = 0.1f;

    [Header("Axis Settings")]
    public bool drawAxes = true;
    public float axisLength = 10f;
    public float axisWidth = 0.1f;
    public int labelsCountOnAxes = 6;
    public Color axesColor = Color.blue;
    public string xAxisLabel = "Axis X";
    public string yAxisLabel = "Axis Y";
    public string zAxisLabel = "Axis Z";
    public Color dataColor = Color.black;
    public float fontSize = 5f;
    public Color textColor = Color.white;

    [Header("Point Settings")]
    public Color currentColor = new Color32(255, 108, 0, 255);
    public Color optimalColor = new Color32(0, 255, 228, 255);
    public Color optimPlaneColor = new Color32(0, 255, 228, 255);
    public Color optimTrajectoryColor = Color.cyan;
    public GameObject point;

    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float horizontalSpeed = 50000f;
    public float verticalSpeed = 50000f;
    public Transform lookCamera;

    [Header("Trajectory Settings")]
    public TMP_Text recommendedText;

    private Vector3[][] matrix;
    private Vector3[][] originalMatrix;
    private Vector3 current;
    private Vector3? optimal;
    private float? minPlane;
    private List<Transform> labels = new();
    private float generalOffset => 0.5f * scaleFactor;
    private float labelsOffset => 1.1f * scaleFactor;
    private GameObject currentPoint;
    private GameObject currentTrajectory;
    private object locked = new();
    public Vector3 currentOptimimum;

    void Rotate()
    {
        float h = -horizontalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
        float v = verticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        transform.Rotate(v, h, 0);
    }

    void Start()
    {
        // SetData(SamplePointsMatrix(), new(100, 8, 0), new(5, 4, 10));
        if (enableRotation)
        {
            var mouse = GetComponent<MouseButton>();
            mouse.hold.AddListener(Rotate);
            var vr = GetComponent<VrButton>();
            vr.press.AddListener(Rotate);
        }
    }

    void Update()
    {
        UpdateText();
    }

    GameObject DrawTrajectory()
    {
        float minY = 0;
        if (optimal.HasValue) { minY = optimal.Value.y; }
        else if (minPlane.HasValue) { minY = minPlane.Value; }

        List<Vector3> vec = new();
        var line = GetNewLineRenderer("trajectory", optimTrajectoryColor, false, true);
        int row = 0, col = 0;

        bool isFound = false;

        for (int i = 0; i < originalMatrix.Length; i++)
        {
            for (int j = 0; j < originalMatrix[i].Length; j++)
            {
                if (originalMatrix[i][j] == current)
                {
                    row = i;
                    col = j;
                    isFound = true;
                    break;
                }
            }
            if (isFound) break;
        }

        List<(int, int)> path;

        if (optimal is not null)
        {
            path = MatrixManipulator.FindPathToMinimum(originalMatrix, row, col);
        }
        else
        {
            path = MatrixManipulator.FindPathToLessThen(originalMatrix, row, col, minPlane.Value);
        }

        int counter = 0;
        foreach (var (cRow, cCol) in path)
        {
            var orValue = originalMatrix[cRow][cCol];
            var value = matrix[cRow][cCol];
            vec.Add(value);
            counter++;
            if (orValue.y <= minY) break;
        }

        var coords = path.Last();
        currentOptimimum = originalMatrix[coords.Item1][coords.Item2];

        if (counter > 1)
        {
            SetRecommendedAction(path[1].Item1, path[1].Item2, row, col);
        }
        else
        {
            recommendedText.text = "Цель управления достигнута";
        }

        line.positionCount = vec.Count;
        line.SetPositions(vec.ToArray());
        return line.gameObject;
    }

    void SetRecommendedAction(int row, int col, int rowCur, int colCur)
    {
        if (recommendedText is null) return;

        if (row == rowCur && col > colCur)
        {
            recommendedText.text = "Увеличьте значение усилия контризгиба";
            return;
        }
        if (row == rowCur && col < colCur)
        {
            recommendedText.text = "Уменьшите значение усилия контризгиба";
            return;
        }
        if (col == colCur && row > rowCur)
        {
            recommendedText.text = "Увеличьте значение перекрещивания";
            return;
        }
        if (col == colCur && row < rowCur)
        {
            recommendedText.text = "Уменьшите значение перекрещивания";
            return;
        }
    }

    Vector3? SafeGetPoint(int row, int col)
    {
        if (row < 0 || row >= originalMatrix.Length || col < 0 || col >= originalMatrix[0].Length)
        {
            return null;
        }

        return originalMatrix[row][col];
    }

    void CreatePlot()
    {
        lock (locked)
        {
            labels = new();
        }

        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }


        if (drawAxes)
        {
            CreateAxesAndText();
        }
        CreatePoints();
        currentPoint = CreatePoint(current, currentColor);
        if (optimal.HasValue)
        {
            CreatePoint(optimal.Value, optimalColor);
        }
        if (minPlane.HasValue)
        {
            CreateMinPlane(minPlane.Value);
        }
        currentTrajectory = DrawTrajectory();
    }



    void CreateMinPlane(float minYPlane)
    {
        var scaledAxis = axisLength * scaleFactor;
        var maxX = originalMatrix.MaxX();
        var maxY = originalMatrix.MaxY();
        var maxZ = originalMatrix.MaxZ();

        var minX = originalMatrix.MinX();
        var minY = originalMatrix.MinY();
        var minZ = originalMatrix.MinZ();

        var yPoint = GetProportion(minYPlane, minY, maxY);

        for (int i = 0; i < labelsCountOnAxes + 2; i++)
        {
            (var _, var point) = GetMidValue(i, minX, maxX, labelsCountOnAxes + 2);
            Vector3 pointOnAxis = new(point, yPoint, -scaledAxis);

            CreateAxis("Min XZ Axis " + i, pointOnAxis, new(point, yPoint, scaledAxis), optimPlaneColor, true);
        }

        for (int i = 0; i < labelsCountOnAxes + 2; i++)
        {
            (var _, var point) = GetMidValue(i, minZ, maxZ, labelsCountOnAxes + 2);
            Vector3 pointOnAxis = new(-scaledAxis, yPoint, point);

            CreateAxis("Min ZX Axis " + i, pointOnAxis, new(scaledAxis, yPoint, point), optimPlaneColor, true);
        }
    }

    GameObject CreatePoint(Vector3 pos, Color color)
    {
        float maxX = originalMatrix.MaxX(), maxY = originalMatrix.MaxY(), maxZ = originalMatrix.MaxZ();
        float minX = originalMatrix.MinX(), minY = originalMatrix.MinY(), minZ = originalMatrix.MinZ();
        var p = Instantiate(point, transform);
        p.layer = gameObject.layer;
        p.transform.localPosition = GetProportionalVector(pos, maxX, minX, maxY, minY, maxZ, minZ);
        p.transform.localScale = new Vector3(scaleFactor / 1.5f, scaleFactor / 1.5f, scaleFactor / 1.5f);
        MeshRenderer renderer = p.GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.color = color;
        renderer.material = newMaterial;
        return p;
    }

    public void UpdateCurrentPoint(Vector3 pos)
    {
        Destroy(currentPoint);
        Destroy(currentTrajectory);
        current = pos;
        currentPoint = CreatePoint(current, currentColor);
        currentTrajectory = DrawTrajectory();
    }

    void CreatePoints()
    {
        var matrix = this.matrix;
        for (int i = 0; i < matrix.Length; i++)
        {
            var lr = GetNewLineRenderer($"Width {i}", dataColor);
            var vec = matrix[i];
            lr.positionCount = vec.Length;
            lr.SetPositions(vec);
        }

        for (int j = 0; j < matrix[0].Length; j++)
        {
            var lr = GetNewLineRenderer($"Height {j}", dataColor);
            var vec = matrix.Select(vec => vec[j]).ToArray();
            lr.positionCount = vec.Length;
            lr.SetPositions(vec);
        }
    }

    public void SetData(Vector3[][] points, Vector3 current, Vector3? optimal, float? min)
    {

        SetMatrix(points);
        SetCurrent(current);
        SetOptimal(optimal);
        SetMinPlane(min);
        CreatePlot();
    }

    void SetMatrix(Vector3[][] matrix)
    {
        this.originalMatrix = matrix;
        this.matrix = ScaledMatrix(matrix);
    }
    void SetCurrent(Vector3 cur) { this.current = cur; }
    void SetOptimal(Vector3? optimal) { this.optimal = optimal; }
    void SetMinPlane(float? min) { this.minPlane = min; }

    Vector3[][] SamplePointsMatrix()
    {
        return new Vector3[][]
        {
            new Vector3[] {new(0, 1, 0), new(5, 4, 0), new(100, 8, 0)},
            new Vector3[] {new(0, 1, 5), new(5, 4, 5), new(100, 8, 5)},
            new Vector3[] {new(0, 1, 10), new(5, 4, 10), new(100, 8, 10)}
        };
    }

    void CreateAxesAndText()
    {
        var scaledAxis = axisLength * scaleFactor;

        Vector3 xAxisStart = new Vector3(-scaledAxis, -scaledAxis, -scaledAxis);
        Vector3 yAxisStart = new Vector3(-scaledAxis, -scaledAxis, -scaledAxis);
        Vector3 zAxisStart = new Vector3(-scaledAxis, -scaledAxis, -scaledAxis);
        Vector3 xAxisEnd = new Vector3(scaledAxis, -scaledAxis, -scaledAxis);
        Vector3 yAxisEnd = new Vector3(-scaledAxis, scaledAxis, -scaledAxis);
        Vector3 zAxisEnd = new Vector3(-scaledAxis, -scaledAxis, scaledAxis);
        CreateAxis("X Axis", xAxisStart, xAxisEnd, axesColor);
        CreateAxis("Y Axis", yAxisStart, yAxisEnd, axesColor);
        CreateAxis("Z Axis", zAxisStart, zAxisEnd, axesColor);

        var maxX = originalMatrix.MaxX();
        var maxY = originalMatrix.MaxY();
        var maxZ = originalMatrix.MaxZ();

        var minX = originalMatrix.MinX();
        var minY = originalMatrix.MinY();
        var minZ = originalMatrix.MinZ();

        labels.Add(GetNewText("xTitle", xAxisLabel, new Vector3(0, -scaledAxis, scaledAxis) + new Vector3(0, -labelsOffset, labelsOffset), true));
        labels.Add(GetNewText("yTitle", yAxisLabel, new Vector3(-scaledAxis, 0, -scaledAxis) + new Vector3(labelsOffset, 0, labelsOffset), true));
        labels.Add(GetNewText("zTitle", zAxisLabel, new Vector3(scaledAxis, -scaledAxis, 0) + new Vector3(labelsOffset, -labelsOffset, 0), true));


        // Axis X
        for (int i = 0; i < labelsCountOnAxes; i++)
        {
            (var val, var point) = GetMidValue(i, minX, maxX, labelsCountOnAxes);
            Vector3 pointOnAxis = new(point, -scaledAxis, -scaledAxis);
            AddMidText(val, pointOnAxis, Axis.X);

            if (i == 0) continue;
            CreateAxis("XZ Axis " + i, pointOnAxis, new(point, -scaledAxis, scaledAxis), axesColor);
            CreateAxis("XY Axis " + i, pointOnAxis, new(point, scaledAxis, -scaledAxis), axesColor);
        }

        // Axis Y
        for (int i = 0; i < labelsCountOnAxes; i++)
        {
            (var val, var point) = GetMidValue(i, minY, maxY, labelsCountOnAxes);
            Vector3 pointOnAxis = new(-scaledAxis, point, -scaledAxis);
            AddMidText(val, pointOnAxis, Axis.Y);

            if (i == 0) continue;
            CreateAxis("YX Axis " + i, pointOnAxis, new(scaledAxis, point, -scaledAxis), axesColor);
            CreateAxis("YZ Axis " + i, pointOnAxis, new(-scaledAxis, point, scaledAxis), axesColor);
        }

        // Axis Z
        for (int i = 0; i < labelsCountOnAxes; i++)
        {
            (var val, var point) = GetMidValue(i, minZ, maxZ, labelsCountOnAxes);
            Vector3 pointOnAxis = new(-scaledAxis, -scaledAxis, point);
            AddMidText(val, pointOnAxis, Axis.Z);

            if (i == 0) continue;
            CreateAxis("ZY Axis " + i, pointOnAxis, new(-scaledAxis, scaledAxis, point), axesColor);
            CreateAxis("ZX Axis " + i, pointOnAxis, new(scaledAxis, -scaledAxis, point), axesColor);
        }
    }

    void CreateAxis(string name, Vector3 start, Vector3 end, Color color, bool thick = false)
    {
        var lr = GetNewLineRenderer(name, color, false, thick);
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    Vector3[][] ScaledMatrix(Vector3[][] mat) 
    {
        var maxX = mat.MaxX();
        var maxY = mat.MaxY();
        var maxZ = mat.MaxZ();

        var minX = mat.MinX();
        var minY = mat.MinY();
        var minZ = mat.MinZ();

        var result = new Vector3[mat.Length][];

        for (int i = 0; i < mat.Length; i++)
        {
            result[i] = new Vector3[mat[i].Length];
            for (int j = 0; j < mat[0].Length; j++)
            {
                result[i][j] = GetProportionalVector(mat[i][j], maxX, minX, maxY, minY, maxZ, minZ);
            }
        }
        return result;
    }

    Vector3 GetProportionalVector(Vector3 vector, float maxX, float minX, float maxY, float minY, float maxZ, float minZ)
    {
        return new Vector3(
                    GetProportion(vector.x, minX, maxX),
                    GetProportion(vector.y, minY, maxY),
                    GetProportion(vector.z, minZ, maxZ));
    }

    float GetProportion(float value, float min, float max)
    {
        var scaledLength = axisLength * scaleFactor;
        var prop = (max - value) / (max - min);
        var point = scaledLength - prop * (2 * scaledLength);
        return point;
    }
    LineRenderer GetNewLineRenderer(string name, Color rendererColor, bool isPlot = true, bool isThick = false)
    {
        GameObject axis = new GameObject(name);
        axis.layer = gameObject.layer;
        axis.transform.position = transform.position;
        axis.transform.SetParent(transform);

        LineRenderer lr = axis.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;

        var mult = isThick ? 1.5f : isPlot ? 0.75f : 1;

        float scalecWidth = axisWidth * mult * scaleFactor;
        lr.startWidth = scalecWidth;
        lr.endWidth = scalecWidth;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = rendererColor;
        axis.transform.localScale = new Vector3(1, 1, 1);
        axis.transform.localRotation = Quaternion.Euler(0, 0, 0);
        return lr;
    }

    Transform GetNewText(string name, string label, Vector3 pos, bool isLabel = false)
    {
        GameObject textObj = new GameObject(name);
        textObj.layer = gameObject.layer;
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = pos;

        var prop = 1f / 3f;
        var fontSize = (isLabel ? this.fontSize * 1.5f : this.fontSize) * scaleFactor;
        float size = label.Length * prop * fontSize;
        TextMeshPro text = textObj.AddComponent<TextMeshPro>();
        var rect = (RectTransform)textObj.transform;
        rect.sizeDelta = new(size * 2f / 10f, size / 2f / 10f);
        text.text = label;
        text.fontSize = fontSize;
        text.color = textColor;
        textObj.transform.localScale = new Vector3(1, 1, 1);
        return textObj.transform;
    }

    //void CreateText()
    //{
    //    var maxX = originalMatrix.MaxX();
    //    var maxY = originalMatrix.MaxY();
    //    var maxZ = originalMatrix.MaxZ();

    //    var yTitleOffset = new Vector3(0, 0.11f, 0);
    //    labels.Add(GetNewText("xTitle", xAxisLabel, ClampedScaledVector(new Vector3(axisLength, 0, 0)) + yTitleOffset, true));
    //    labels.Add(GetNewText("yTitle", yAxisLabel, ClampedScaledVector(new Vector3(0, axisLength, 0)) + yTitleOffset, true));
    //    labels.Add(GetNewText("zTitle", zAxisLabel, ClampedScaledVector(new Vector3(0, 0, axisLength)) + yTitleOffset, true));

    //    labels.Add(GetNewText("center", "0", new Vector3(generalOffset, generalOffset, generalOffset)));

    //    // Axis X
    //    for (int i = 0; i < labelsCountOnAxes; i++)
    //    {
    //        (var val, var point) = GetMidValue(i, -maxX, maxX);
    //        AddMidText(val, new(point, 0, 0), Axis.X);
    //    }

    //    // Axis Y
    //    for (int i = 0; i < labelsCountOnAxes; i++)
    //    {
    //        (var val, var point) = GetMidValue(i, -maxY, maxY);
    //        AddMidText(val, new(0, point, 0), Axis.Y);
    //    }

    //    // Axis Z
    //    for (int i = 0; i < labelsCountOnAxes; i++)
    //    {
    //        (var val, var point) = GetMidValue(i, -maxZ, maxZ);
    //        AddMidText(val, new(0, 0, point), Axis.Z);
    //    }
    //}

    void UpdateText()
    {
        lock (locked)
        {
            var camera = lookCamera.transform;
            foreach (var label in labels)
            {
                label.rotation = Quaternion.LookRotation(label.position - camera.position);
            }
        }
    }

    void AddMidText(float value, Vector3 point, Axis axis)
    {
        Vector3 offset = axis switch
        {
            Axis.X => new(0, 0, -generalOffset),
            Axis.Y => new(-generalOffset, 0, -generalOffset),
            Axis.Z => new(-generalOffset, 0, 0),
        };
        labels.Add(GetNewText(point.ToString(), value.ToString("F"), point + offset));
    }

    (float, float) GetMidValue(int index, float min, float max, int count)
    {
        var value = min + ((max - min) / (count - 1) * index);
        var point = GetProportion(value, min, max);
        return (value, point);
    }
}