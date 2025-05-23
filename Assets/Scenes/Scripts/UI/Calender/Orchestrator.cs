using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Orchestrator : MonoBehaviour
{
    private long scenarioId;
    private double curCross;
    private double curCurve;
    private Scenario scenario;
    private List<ProfilePoint> avFilmProfile;
    private List<ProfilePoint> currentProfile;
    private double delDiff;
    private bool isDelMore;

    private List<System.Numerics.Vector2> profCross;
    private List<System.Numerics.Vector2> profCurve;
    private List<List<Vector3>> profAll;
    private Vector3 cur;
    private Vector3 min;

    public TMP_Text sceneParams;
    public TMP_Text sceneTask;
    public TMP_Text crossParams;
    public TMP_Text curveParams;

    public List<TMP_Text> curCrossText;
    public List<TMP_Text> curCurveText;
    public TMP_Text curDeltaText;

    public TMP_Text optimCrossText;
    public TMP_Text optimCurveText;
    public TMP_Text optimDeltaText;

    public List<UnityEngine.UI.Button> curveUp;
    public List<UnityEngine.UI.Button> curveDown;

    public List<UnityEngine.UI.Button> crossUp;
    public List<UnityEngine.UI.Button> crossDown;

    public DataPlotter graph3d;
    public WindowGraph crossGraph;
    public WindowGraph curveGraph;
    public WindowGraph averageGraph;
    public WindowGraph currentGraph;
    public WindowGraph startGraph;
    public CurrTimeUpdate timeUpdate;

    public TableManager crossTable;
    public TableManager curveTable;
    public SquareTableManager bothTable;
    public TableManager protocol;

    public TableManager protocolInstr;
    public WindowGraph crossGraphInstr;
    public WindowGraph curveGraphInstr;
    public GameObject graph3dInstr;
    public TMP_Text resultInstr;

    public MouseButton endLearningButton;
    public GameObject UI;
    //public GameObject curUI;

    public TMP_Text optimTitle;
    public TMP_Text recomTitle;

    public GameObject player;
    public Vector3 startPlayerPosition;

    void Awake()
    {
        foreach (var curveButUp in curveUp)
        {
            curveButUp.onClick.AddListener(CurveUp);
        }
        foreach (var curveButDown in curveDown)
        {
            curveButDown.onClick.AddListener(CurveDown);
        }
        foreach (var crossButDown in crossDown)
        {
            crossButDown.onClick.AddListener(CrossDown);
        }
        foreach (var crossButUp in crossUp)
        {
            crossButUp.onClick.AddListener(CrossUp);
        }
        endLearningButton.click.AddListener(EndLearning);
    }

    private void EndLearning()
    {
        //curUI.SetActive(false);
        UI.SetActive(true);
        Debug.Log("end");
    }

    private void OnDestroy()
    {
        foreach (var curveButUp in curveUp)
        {
            curveButUp.onClick.RemoveListener(CurveUp);
        }
        foreach (var curveButDown in curveDown)
        {
            curveButDown.onClick.RemoveListener(CurveDown);
        }
        foreach (var crossButDown in crossDown)
        {
            crossButDown.onClick.RemoveListener(CrossDown);
        }
        foreach (var crossButUp in crossUp)
        {
            crossButUp.onClick.RemoveListener(CrossUp);
        }
    }

    public void Init(long scenarioId)
    {
        player.transform.position = startPlayerPosition;
        Debug.Log("startstart");
        crossGraph.Clear();
        curveGraph.Clear();
        averageGraph.Clear();
        currentGraph.Clear();
        startGraph.Clear();
        crossTable.Clear();
        curveTable.Clear();
        bothTable.Clear();
        protocol.Clear();
        protocolInstr.Clear();
        crossGraphInstr.Clear();
        curveGraphInstr.Clear();
        graph3dInstr.SetActive(true);

        this.scenarioId = scenarioId;
        using var ctx = new CalenderContext();
        scenario = ctx.Scenarios.Include(s => s.RollSettings).Include(s => s.FilmProfileCluster).ThenInclude(pc => pc.FilmProfiles).Include(s => s.FilmProfileCluster.Film).AsNoTracking().FirstOrDefault(s => s.Id == scenarioId);
        if (scenario is null) return;
        timeUpdate.SetTime(TimeSpan.FromMinutes(scenario.Minutes));
        if (scenario.TableSkipStep <= 0) scenario.TableSkipStep = 1;
        SetScenario();
        GetAverageProfile();
        SetAverageProfileToCurrent();
        DrawAverageGraph();
        DrawCurrentGraph();
        GetAverageProfileWithoutControl();
        Recalc3D();
        if (scenario.IsRange)
        {
            UpdateOptCross();
            UpdateOptCurve();
            UpdateOptDelta();
        }
        RecalcCross();
        RecalcCurve();
    }

    void GetNewCurrent()
    {
        var thickAv = currentProfile.Sum(s => s.Thickness) / currentProfile.Count;
        var delDiffHalf = delDiff / 2;
        foreach (var point in currentProfile)
        {
            if (point.Thickness > thickAv)
            {
                if (isDelMore) point.Thickness += delDiffHalf;
                else point.Thickness -= delDiffHalf;
            }
            if (point.Thickness < thickAv)
            {
                if (isDelMore) point.Thickness -= delDiffHalf;
                else point.Thickness += delDiffHalf;
            }
        }
        //var del = currentProfile.Max(x => x.Thickness) - currentProfile.Min(x => x.Thickness);
        //Debug.Log("Delta " + del);
    }

    void SetAverageProfileToCurrent()
    {
        currentProfile = new List<ProfilePoint>();
        foreach (ProfilePoint point in avFilmProfile)
        {
            currentProfile.Add(new ProfilePoint
            {
                Id = point.Id,
                Thickness = point.Thickness,
                WidthPoint = point.WidthPoint,
            });
        }
    }
    public void SetScenario()
    {
        curCurve = scenario.FilmProfileCluster.CurveStart;
        curCross = scenario.FilmProfileCluster.CrossStart;
        string taskText = scenario.IsRange ? $"Разнотолщинность пленки не превышающая предельно допустимого значение\nS<sup>max</sup> = {scenario.ThicknessMax:F} мкм - Предельно допустимая разнотолщинность" : "Минимальная разнотолщинность пленки";
        sceneTask.text = taskText;
        sceneParams.text = $"{scenario.AveragedProfilesCount}\n{scenario.AveragedProfileWeight:F}\n{scenario.LastProfileWeight:F}\n{scenario.FilmProfileCluster.CrossStart:F}\n{scenario.FilmProfileCluster.CurveStart:F}";
        crossParams.text = $"x<sup>min</sup>={scenario.FilmProfileCluster.Film.CrossMin:F} мм, x<sup>max</sup>={scenario.FilmProfileCluster.Film.CrossMax:F} мм, delta_x={scenario.FilmProfileCluster.Film.CrossDelta:F} мм";
        curveParams.text = $"r<sup>min</sup>={scenario.FilmProfileCluster.Film.CurveMin:F} Н, r<sup>max</sup>={scenario.FilmProfileCluster.Film.CurveMax:F} Н, delta_r={scenario.FilmProfileCluster.Film.CurveDelta:F} Н";
        if (scenario.IsRange)
        {
            optimTitle.text = "Управляющие воздействия подходящей разнотолщинности,\nближайшей к текущим управляющим воздействиям:";
            recomTitle.text = "Для достижения заданной разнотолщинности:";
        }
        else
        {
            optimTitle.text = "Оптимальные управляющие воздействия:";
            recomTitle.text = "Для достижения минимальной разнотолщинности:";
        }
        UpdateCurCross();
        UpdateCurCurve();
    }

    void UpdateCurCross() 
    {
        foreach (var curCrossT in curCrossText)
        {
            curCrossT.text = curCross.ToString("F");
        }
    }
    void UpdateCurCurve() 
    { 
        foreach (var curCurveT in curCurveText)
        {
            curCurveT.text = curCurve.ToString("F");
        }
    }
    void UpdateCurDelta() 
    { 
        curDeltaText.text = $"{cur.y:F} мкм";
        resultInstr.text = $"{curCross:F}\n{curCurve:F}\n{cur.y:F}";
    }
    void UpdateOptCross() 
    { 
        if (scenario.IsRange)
        {
            optimCrossText.text = $"Перекрещивание - {graph3d.currentOptimimum.z:F} мм";
        }
        else
        {
            optimCrossText.text = $"Перекрещивание - {min.z:F} мм";
        }
    }
    void UpdateOptCurve() 
    {
        if (scenario.IsRange)
        {
            optimCurveText.text = $"Усилие контризгиба - {graph3d.currentOptimimum.x:F} Н";
        }
        else
        {
            optimCurveText.text = $"Усилие контризгиба - {min.x:F} Н";
        }
    }
    void UpdateOptDelta() 
    { 
        if (scenario.IsRange)
        {
            optimDeltaText.text = $"Разнотолщинность - {graph3d.currentOptimimum.y:F} мкм < {scenario.ThicknessMax:F}";
        }
        else
        {
            optimDeltaText.text = $"Минимальная разнотолщинность - {min.y:F} мкм";
        }
    }

    public void CrossUp()
    {
        if (!CheckCross(true)) return;
        curCross += scenario.FilmProfileCluster.Film.CrossDelta;
        UpdatePoint();
        UpdateCurCross();
        AddLog();
        GetNewCurrent();
        DrawCurrentGraph();

        UpdateOptCross();
        UpdateOptCurve();
        UpdateOptDelta();
    }

    public void CrossDown()
    {
        if (!CheckCross(false)) return;
        curCross -= scenario.FilmProfileCluster.Film.CrossDelta;
        UpdatePoint();
        UpdateCurCross();
        AddLog();
        GetNewCurrent();
        DrawCurrentGraph();

        UpdateOptCross();
        UpdateOptCurve();
        UpdateOptDelta();
    }

    public void CurveUp()
    {
        if (!CheckCurve(true)) return;
        curCurve += scenario.FilmProfileCluster.Film.CurveDelta;
        UpdatePoint();
        UpdateCurCurve();
        AddLog();
        GetNewCurrent();
        DrawCurrentGraph();

        UpdateOptCross();
        UpdateOptCurve();
        UpdateOptDelta();
    }

    public void CurveDown()
    {
        if (!CheckCurve(false)) return;
        curCurve -= scenario.FilmProfileCluster.Film.CurveDelta;
        UpdatePoint();
        UpdateCurCurve();
        AddLog();
        GetNewCurrent();
        DrawCurrentGraph();

        UpdateOptCross();
        UpdateOptCurve();
        UpdateOptDelta();
    }

    public void AddLog()
    {
        var minData = scenario.IsRange
            ? (cur.y - scenario.ThicknessMax) / scenario.ThicknessMax * 100
            : (cur.y - min.y) / min.y * 100;

        var minVal = scenario.IsRange
            ? cur.y - scenario.ThicknessMax
            : cur.y - min.y;

        var data = new List<object>
        {
            timeUpdate.GetTimeStr(),
            curCross,
            curCurve,
            cur.y,
            minData,
            minVal
        };

        protocol.AddData(data);
        protocolInstr.AddData(data);
    }

    bool CheckCross(bool more)
    {
        var cur = curCross + (more ? scenario.FilmProfileCluster.Film.CrossDelta : -scenario.FilmProfileCluster.Film.CrossDelta);
        return cur >= scenario.FilmProfileCluster.Film.CrossMin && cur <= scenario.FilmProfileCluster.Film.CrossMax;
    }

    bool CheckCurve(bool more)
    {
        var cur = curCurve + (more ? scenario.FilmProfileCluster.Film.CurveDelta : -scenario.FilmProfileCluster.Film.CurveDelta);
        return cur >= scenario.FilmProfileCluster.Film.CurveMin && cur <= scenario.FilmProfileCluster.Film.CurveMax;
    }

    void DrawAverageGraph()
    {
        averageGraph.Clear();
        foreach (var point in avFilmProfile)
        {
            averageGraph.AddData(new System.Numerics.Vector2((float)point.WidthPoint, (float)point.Thickness));
        }
        startGraph.Clear();
        foreach (var point in scenario.FilmProfileCluster.FilmProfiles.Last().Profile)
        {
            startGraph.AddData(new System.Numerics.Vector2((float)point.WidthPoint, (float)point.Thickness));
        }
    }

    void DrawCurrentGraph()
    {
        currentGraph.Clear();
        foreach (var point in currentProfile)
        {
            currentGraph.AddData(new System.Numerics.Vector2((float)point.WidthPoint, (float)point.Thickness));
        }
    }

    void UpdatePoint()
    {
        GetCur();
        graph3d.UpdateCurrentPoint(cur);
        UpdateCurDelta();
    }

    void Recalc3D()
    {
        FillAll();
        if (scenario.IsRange)
        {
            graph3d.SetData(profAll.Select(vec => vec.ToArray()).ToArray(), cur, null, (float)scenario.ThicknessMax);
        }
        else
        {
            graph3d.SetData(profAll.Select(vec => vec.ToArray()).ToArray(), cur, min, null);
        }
    }


    void RecalcCross()
    {
        crossGraph.Clear();
        crossGraphInstr.Clear();
        FillCross();
        if (scenario.IsRange)
        {
            crossGraph.SetYMaxLine((float)scenario.ThicknessMax);
            crossGraphInstr.SetYMaxLine((float)scenario.ThicknessMax);
        }
        foreach (var point in profCross)
        {
            crossGraph.AddData(point);
            crossGraphInstr.AddData(point);
        }
    }

    void RecalcCurve()
    {
        curveGraph.Clear();
        curveGraphInstr.Clear();
        FillCurve();
        if (scenario.IsRange)
        {
            curveGraph.SetYMaxLine((float)scenario.ThicknessMax);
            curveGraphInstr.SetYMaxLine((float)scenario.ThicknessMax);
        }
        foreach (var point in profCurve)
        {
            curveGraph.AddData(point);
            curveGraphInstr.AddData(point);
        }
    }

    void GetAverageProfile()
    {
        avFilmProfile = new();
        // Текущий
        var curProfile = scenario.FilmProfileCluster.FilmProfiles.Last().Profile;
        var coef = scenario.LastProfileWeight;
        curProfile = curProfile.Select(p => new ProfilePoint { Id = p.Id, Thickness = p.Thickness * coef, WidthPoint = p.WidthPoint }).ToList();

        if (scenario.AveragedProfilesCount <= 1 && scenario.AveragedProfileWeight == 0 && scenario.FilmProfileCluster.FilmProfiles.Count == 1)
        {
            avFilmProfile = curProfile;
            return;
        }

        // Усредненный
        var withoutCurCount = scenario.FilmProfileCluster.FilmProfiles.Count - 1;
        var avCoef = scenario.AveragedProfileWeight;
        var avCount = (scenario.AveragedProfilesCount - 1) < withoutCurCount ? (scenario.AveragedProfilesCount - 1) : withoutCurCount;
        var profiles = scenario.FilmProfileCluster.FilmProfiles.ToArray();
        var averagedProfile = profiles[0].Profile.Select(p => new ProfilePoint { Id = p.Id, Thickness = p.Thickness, WidthPoint = p.WidthPoint }).ToList();
        for (int i = 1; i < avCount; i++)
        {
            for (int j = 0; j < averagedProfile.Count; j++)
            {
                averagedProfile[j].Thickness = averagedProfile[j].Thickness + profiles[i].Profile[j].Thickness;
            }
        }
        for (int i = 0; i < averagedProfile.Count; i++)
        {
            averagedProfile[i].Thickness = (averagedProfile[i].Thickness / avCount) * avCoef;
        }

        // Вместе
        for (int i = 0; i < curProfile.Count; i++)
        {
            var point = new ProfilePoint { Id = curProfile[i].Id, Thickness = curProfile[i].Thickness, WidthPoint = curProfile[i].WidthPoint };
            point.Thickness += averagedProfile[i].Thickness;
            avFilmProfile.Add(point);
        }
    }

    void GetAverageProfileWithoutControl()
    {
        var widthConst = (scenario.RollSettings.Width - scenario.FilmProfileCluster.Width) / 2;
        foreach (var point in avFilmProfile)
        {
            var sCross = CalenderControlActions.Cross(curCross, scenario.FilmProfileCluster.Width, scenario.RollSettings.Width, scenario.RollSettings.BarrelDiameter, point.WidthPoint);
            var sCurve = CalenderControlActions.Curve(curCurve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint + widthConst, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            point.Thickness = point.Thickness - sCross - sCurve;
        }

    }

    void FillCross()
    {
        int count = 0;
        profCross = new();
        for (var cross = scenario.FilmProfileCluster.Film.CrossMin; cross <= scenario.FilmProfileCluster.Film.CrossMax; cross += scenario.FilmProfileCluster.Film.CrossDelta)
        {
            var delCross = GetCross(cross);
            bool wasCross = false;
            profCross.Add(new((float)cross, (float)delCross));
            if (count == 0)
            {
                crossTable.AddData(new List<object> { cross, delCross });
            }
            count = (count + 1) % scenario.TableSkipStep;
            if (!wasCross && cross + scenario.FilmProfileCluster.Film.CrossDelta > scenario.FilmProfileCluster.Film.CrossMax)
            {
                crossTable.AddData(new List<object> { cross, delCross });
            }
        }
    }

    void FillCurve()
    {
        int count = 0;
        profCurve = new();
        for (var curve = scenario.FilmProfileCluster.Film.CurveMin; curve <= scenario.FilmProfileCluster.Film.CurveMax; curve += scenario.FilmProfileCluster.Film.CurveDelta)
        {
            var delCurve = GetCurve(curve);
            bool wasCurve = false;
            profCurve.Add(new((float)curve, (float)delCurve));
            if (count == 0)
            {
                curveTable.AddData(new List<object> { curve, delCurve });
                wasCurve = true;
            }
            count = (count + 1) % scenario.TableSkipStep;
            if (!wasCurve && curve + scenario.FilmProfileCluster.Film.CurveDelta > scenario.FilmProfileCluster.Film.CurveMax)
            {
                curveTable.AddData(new List<object> { curve, delCurve });
            }
        }
    }

    void FillAll()
    {
        min = new(0, float.MaxValue, 0);
        profAll = new();

        // Первая строка таблицы
        List<object> start = new() { "x, мм \\ r, Н" };
        int stc = 0;
        for (var curve = scenario.FilmProfileCluster.Film.CurveMin; curve <= scenario.FilmProfileCluster.Film.CurveMax; curve += scenario.FilmProfileCluster.Film.CurveDelta)
        {
            bool isAdded = false;
            if (stc == 0)
            {
                start.Add(curve);
                isAdded = true;
            }
            stc = (stc + 1) % scenario.TableSkipStep;
            if (!isAdded && curve + scenario.FilmProfileCluster.Film.CurveDelta > scenario.FilmProfileCluster.Film.CurveMax)
            {
                start.Add(curve);
            }
        }
        bothTable.AddData(start);

        int countCol = 0;
        for (var cross = scenario.FilmProfileCluster.Film.CrossMin; cross <= scenario.FilmProfileCluster.Film.CrossMax; cross += scenario.FilmProfileCluster.Film.CrossDelta)
        {
            bool wasCross = false;
            var vec = new List<Vector3>();
            var tableRow = new List<double>() { cross };
            int countRow = 0;
            for (var curve = scenario.FilmProfileCluster.Film.CurveMin; curve <= scenario.FilmProfileCluster.Film.CurveMax; curve += scenario.FilmProfileCluster.Film.CurveDelta)
            {
                bool wasCurve = false;
                var del = GetAll(cross, curve);
                var point = new Vector3((float)curve, (float)del, (float)cross);
                if (point.y < min.y) min = point;

                vec.Add(point);
                if (countRow == 0)
                {
                    tableRow.Add(del);
                    wasCurve = true;
                }
                countRow = (countRow + 1) % scenario.TableSkipStep;
                if (!wasCurve && curve + scenario.FilmProfileCluster.Film.CurveDelta > scenario.FilmProfileCluster.Film.CurveMax)
                {
                    tableRow.Add(del);
                }
            }
            if (countCol == 0)
            {
                bothTable.AddData(tableRow);
                wasCross = true;
            }
            countCol = (countCol + 1) % scenario.TableSkipStep;
            if (!wasCross && cross + scenario.FilmProfileCluster.Film.CrossDelta > scenario.FilmProfileCluster.Film.CrossMax)
            {
                bothTable.AddData(tableRow);
            }
            profAll.Add(vec);
        }
        GetCur();
        if (!scenario.IsRange)
        {
            UpdateOptCross();
            UpdateOptCurve();
            UpdateOptDelta();
        }
        UpdateCurDelta();
    }


    //void GetTableRange()
    //{
    //    var tableRange = MatrixManipulator.FindLargestRectangle(profAll, (float)scenario.ThicknessMax);
    //    if (tableRange.Length != 4)
    //    {
    //        minCross = min.z;
    //        maxCross = min.z;
    //        minCurve = min.x;
    //        maxCurve = min.x;
    //        return;
    //    }


    //    minCross = profAll[tableRange[0]][tableRange[1]].z;
    //    maxCross = profAll[tableRange[2]][tableRange[1]].z;
    //    minCurve = profAll[tableRange[0]][tableRange[1]].x;
    //    maxCurve = profAll[tableRange[0]][tableRange[3]].x;

    //    int top = tableRange[0], bottom = tableRange[2], left = tableRange[1], right = tableRange[3];
    //    while (top % scenario.TableSkipStep != 0)
    //    {
    //        top++;
    //    }

    //    while (bottom % scenario.TableSkipStep != 0)
    //    {
    //        bottom--;
    //    }

    //    while (left % scenario.TableSkipStep != 0)
    //    {
    //        left++;
    //    }

    //    while (right % scenario.TableSkipStep != 0)
    //    {
    //        right--;
    //    }

    //    if (top > bottom || left > right) return;
    //    top = top % scenario.TableSkipStep + top / scenario.TableSkipStep;
    //    bottom = bottom % scenario.TableSkipStep + bottom / scenario.TableSkipStep;
    //    left = left % scenario.TableSkipStep + left / scenario.TableSkipStep;
    //    right = right % scenario.TableSkipStep + right / scenario.TableSkipStep;
    //    bothTable.SetColor(top, bottom, left, right);
    //}

    void GetCur()
    {
        for (var cross = scenario.FilmProfileCluster.Film.CrossMin; cross <= scenario.FilmProfileCluster.Film.CrossMax; cross += scenario.FilmProfileCluster.Film.CrossDelta)
        {
            for (var curve = scenario.FilmProfileCluster.Film.CurveMin; curve <= scenario.FilmProfileCluster.Film.CurveMax; curve += scenario.FilmProfileCluster.Film.CurveDelta)
            {
                if (Math.Abs(curCross - cross) < (scenario.FilmProfileCluster.Film.CrossDelta * 0.1)
                    &&
                    Math.Abs(curCurve - curve) < (scenario.FilmProfileCluster.Film.CurveDelta * 0.1))
                {
                    var del = GetAll(cross, curve);
                    isDelMore = del > cur.y;
                    delDiff = cur == default ? del : Math.Abs(cur.y - del);
                    cur = new Vector3((float)curve, (float)del, (float)cross);
                    return;
                }
            }
        }
    }

    double GetCross(double cross)
    {
        var onlyCross = avFilmProfile.Select(point =>
        {
            var sCross = CalenderControlActions.Cross(cross, scenario.FilmProfileCluster.Width, scenario.RollSettings.Width, scenario.RollSettings.BarrelDiameter, point.WidthPoint);
            return point.Thickness + sCross;
        }).ToArray();

        return onlyCross.Max() - onlyCross.Min();
    }

    double GetCurve(double curve)
    {
        var widthConst = (scenario.RollSettings.Width - scenario.FilmProfileCluster.Width) / 2;
        var onlyCurve = avFilmProfile.Select(point =>
        {
            var sCurve = CalenderControlActions.Curve(curve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint + widthConst, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            return point.Thickness + sCurve;
        }).ToArray();
        
        return onlyCurve.Max() - onlyCurve.Min();
    }

    double GetAll(double cross, double curve)
    {
        var widthConst = (scenario.RollSettings.Width - scenario.FilmProfileCluster.Width) / 2;
        var all = avFilmProfile.Select(point =>
        {
            var sCross = CalenderControlActions.Cross(cross, scenario.FilmProfileCluster.Width, scenario.RollSettings.Width, scenario.RollSettings.BarrelDiameter, point.WidthPoint);
            var sCurve = CalenderControlActions.Curve(curve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint + widthConst, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            return point.Thickness + sCross + sCurve;
        }).ToArray();

        return all.Max() - all.Min();
    }
}
