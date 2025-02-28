using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    private List<System.Numerics.Vector2> profCross;
    private List<System.Numerics.Vector2> profCurve;
    private List<List<Vector3>> profAll;
    private Vector3 cur;
    private Vector3 min;

    public TMP_Text sceneParams;
    public TMP_Text sceneTask;
    public TMP_Text crossParams;
    public TMP_Text curveParams;

    public TMP_Text curCrossText;
    public TMP_Text curCurveText;
    public TMP_Text curDeltaText;

    public TMP_Text optimCrossText;
    public TMP_Text optimCurveText;
    public TMP_Text optimDeltaText;

    public Button curveUp;
    public Button curveDown;

    public Button crossUp;
    public Button crossDown;

    public DataPlotter graph3d;
    public WindowGraph crossGraph;
    public WindowGraph curveGraph;
    public WindowGraph averageGraph;

    void Awake()
    {
        curveUp.onClick.AddListener(CurveUp);
        curveDown.onClick.AddListener(CurveDown);
        crossDown.onClick.AddListener(CrossDown);
        crossUp.onClick.AddListener(CrossUp);
    }

    private void OnDestroy()
    {
        curveUp.onClick.RemoveListener(CurveUp);
        curveDown.onClick.RemoveListener(CurveDown);
        crossDown.onClick.RemoveListener(CrossDown);
        crossUp.onClick.RemoveListener(CrossUp);
    }

    public void Init(long scenarioId)
    {
        this.scenarioId = scenarioId;
        using var ctx = new CalenderContext();
        scenario = ctx.Scenarios.Include(s => s.RollSettings).Include(s => s.FilmProfileCluster).ThenInclude(pc => pc.FilmProfiles).AsNoTracking().FirstOrDefault(s => s.Id == scenarioId);
        if (scenario is null) return;
        SetScenario();
        GetAverageProfile();
        DrawAverageGraph();
        GetAverageProfileWithoutControl();
        Recalc3D();
        RecalcCross();
        RecalcCurve();
    }

    public void SetScenario()
    {
        curCurve = scenario.FilmProfileCluster.CurveStart;
        curCross = scenario.FilmProfileCluster.CrossStart;
        string taskText = scenario.IsRange ? $"Разнотолщинность пленки не превышающая предельно допустимого значение\nS<sup>max</sup> = {scenario.ThicknessMax:F} мкм - Предельно допустимая разнотолщинность" : "Минимальная разнотолщинность пленки";
        sceneTask.text = taskText;
        sceneParams.text = $"{scenario.AveragedProfilesCount}\n{scenario.AveragedProfileWeight:F}\n{scenario.LastProfileWeight:F}\n{scenario.FilmProfileCluster.CrossStart:F}\n{scenario.FilmProfileCluster.CurveStart:F}";
        crossParams.text = $"X<sup>min</sup>={scenario.CrossMin:F} мм, X<sup>max</sup>={scenario.CrossMax:F} мм, deltaX={scenario.CrossDelta:F} мм";
        curveParams.text = $"R<sup>min</sup>={scenario.CurveMin:F} Н, R<sup>max</sup>={scenario.CurveMax:F} Н, deltaR={scenario.CurveDelta:F} Н";
        UpdateCurCross();
        UpdateCurCurve();
    }

    void UpdateCurCross() { curCrossText.text = $"Перекрещивание - {curCross:F} мм"; }
    void UpdateCurCurve() { curCurveText.text = $"Контризгиб - {curCurve:F} Н"; }
    void UpdateCurDelta() { curDeltaText.text = $"Разнотолщинность - {cur.y:F} мкм"; }
    void UpdateOptCross() { optimCrossText.text = $"Перекрещивание - {min.z:F} мм"; }
    void UpdateOptCurve() { optimCurveText.text = $"Контризгиб - {min.x:F} Н"; }
    void UpdateOptDelta() { optimDeltaText.text = $"Разнотолщинность - {min.y:F} мкм"; }

    public void CrossUp()
    {
        if (!CheckCross(true)) return;
        curCross += scenario.CrossDelta;
        UpdatePoint();
        UpdateCurCross();
    }

    public void CrossDown()
    {
        if (!CheckCross(false)) return;
        curCross -= scenario.CrossDelta;
        UpdatePoint();
        UpdateCurCross();
    }

    public void CurveUp()
    {
        if (!CheckCurve(true)) return;
        curCurve += scenario.CurveDelta;
        UpdatePoint();
        UpdateCurCurve();
    }

    public void CurveDown()
    {
        if (!CheckCurve(false)) return;
        curCurve -= scenario.CurveDelta;
        UpdatePoint();
        UpdateCurCurve();
    }

    bool CheckCross(bool more)
    {
        var cur = curCross + (more ? scenario.CrossDelta : -scenario.CrossDelta);
        return cur >= scenario.CrossMin && cur <= scenario.CrossMax;
    }

    bool CheckCurve(bool more)
    {
        var cur = curCurve + (more ? scenario.CurveDelta : -scenario.CurveDelta);
        return cur >= scenario.CurveMin && cur <= scenario.CurveMax;
    }

    void DrawAverageGraph()
    {
        averageGraph.Clear();
        foreach (var point in avFilmProfile)
        {
            averageGraph.AddData(new System.Numerics.Vector2((float)point.WidthPoint, (float)point.Thickness));
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
        graph3d.SetData(profAll.Select(vec => vec.ToArray()).ToArray(), cur, min);
    }


    void RecalcCross()
    {
        crossGraph.Clear();
        FillCross();
        foreach (var point in profCross)
        {
            crossGraph.AddData(point);
        }
    }

    void RecalcCurve()
    {
        curveGraph.Clear();
        FillCurve();
        foreach (var point in profCurve)
        {
            curveGraph.AddData(point);
        }
    }

    void GetAverageProfile()
    {
        avFilmProfile = new();
        // Текущий
        var curProfile = scenario.FilmProfileCluster.FilmProfiles.Last().Profile;
        var coef = scenario.LastProfileWeight;
        curProfile = curProfile.Select(p => new ProfilePoint { Id = p.Id, Thickness = p.Thickness * coef, WidthPoint = p.WidthPoint }).ToList();

        if (scenario.AveragedProfilesCount <= 0 && scenario.AveragedProfileWeight == 0 && scenario.FilmProfileCluster.FilmProfiles.Count == 1)
        {
            avFilmProfile = curProfile;
            return;
        }

        // Усредненный
        var withoutCurCount = scenario.FilmProfileCluster.FilmProfiles.Count - 1;
        var avCoef = scenario.AveragedProfileWeight;
        var avCount = scenario.AveragedProfilesCount < withoutCurCount ? scenario.AveragedProfilesCount : withoutCurCount;
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
        foreach (var point in avFilmProfile)
        {
            var sCross = CalenderControlActions.Cross(curCross, scenario.FilmProfileCluster.Width, scenario.RollSettings.Width, scenario.RollSettings.BarrelDiameter, point.WidthPoint);
            var sCurve = CalenderControlActions.Curve(curCurve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            point.Thickness = point.Thickness - sCross - sCurve;
        }

    }

    void FillCross()
    {
        profCross = new();
        for (var cross = scenario.CrossMin; cross <= scenario.CrossMax; cross += scenario.CrossDelta)
        {
            var delCross = GetCross(cross);
            profCross.Add(new((float)cross, (float)delCross));
        }
    }

    void FillCurve()
    {
        profCurve = new();
        for (var curve = scenario.CurveMin; curve <= scenario.CurveMax; curve += scenario.CurveDelta)
        {
            var delCurve = GetCurve(curve);
            profCurve.Add(new((float)curve, (float)delCurve));
        }
    }

    void FillAll()
    {
        min = new(0, float.MaxValue, 0);
        profAll = new();
        for (var cross = scenario.CrossMin; cross <= scenario.CrossMax; cross += scenario.CrossDelta)
        {
            var vec = new List<Vector3>();
            for (var curve = scenario.CurveMin; curve <= scenario.CurveMax; curve += scenario.CurveDelta)
            {
                var del = GetAll(cross, curve);
                var point = new Vector3((float)curve, (float)del, (float)cross);
                if (point.y < min.y) min = point;
                vec.Add(point);
            }
            profAll.Add(vec);
        }
        GetCur();
        UpdateOptCross();
        UpdateOptCurve();
        UpdateOptDelta();
        UpdateCurDelta();
    }

    void GetCur()
    {
        for (var cross = scenario.CrossMin; cross <= scenario.CrossMax; cross += scenario.CrossDelta)
        {
            for (var curve = scenario.CurveMin; curve <= scenario.CurveMax; curve += scenario.CurveDelta)
            {
                if (Math.Abs(curCross - cross) < scenario.CrossDelta 
                    &&
                    Math.Abs(curCurve - curve) < scenario.CurveDelta)
                {
                    var del = GetAll(cross, curve);
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
        var onlyCurve = avFilmProfile.Select(point =>
        {
            var sCurve = CalenderControlActions.Curve(curve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            return point.Thickness + sCurve;
        }).ToArray();
        
        return onlyCurve.Max() - onlyCurve.Min();
    }

    double GetAll(double cross, double curve)
    {
        var all = avFilmProfile.Select(point =>
        {
            var sCross = CalenderControlActions.Cross(cross, scenario.FilmProfileCluster.Width, scenario.RollSettings.Width, scenario.RollSettings.BarrelDiameter, point.WidthPoint);
            var sCurve = CalenderControlActions.Curve(curve, scenario.RollSettings.SecondDistance, scenario.RollSettings.FirstDistance, point.WidthPoint, scenario.RollSettings.Width, scenario.RollSettings.Elasticity, scenario.RollSettings.BarrelDiameter, scenario.RollSettings.NeckDiameter, scenario.RollSettings.HoleDiameter);
            return point.Thickness + sCross + sCurve;
        }).ToArray();

        return all.Max() - all.Min();
    }
}
