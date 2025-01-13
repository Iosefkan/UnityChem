﻿using Database;
using Extruder;
using Microsoft.EntityFrameworkCore;
using Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Types;
using UnityEngine;
using UnityEngine.UI;
using Vector = System.Numerics.Vector2;

public class Recalculates : MonoBehaviour
{
    [SerializeField] private EntryValuePanel _entrPanel;
    [SerializeField] private CurrTimeUpdate _operTime;

    [SerializeField] private TMP_Text _shnekSpeed;
    [SerializeField] private TMP_Text _shnekSpeedTarget;
    [SerializeField] private TMP_Text _voronkaSpeed;
    [SerializeField] private TMP_Text _voronkaSpeedTarget;
    [SerializeField] private TMP_Text _voronkaFr;
    [SerializeField] private TMP_Text _knifeSpeed;
    [SerializeField] private TMP_Text _knifeSpeedTarget;
    [SerializeField] private TMP_Text _knifeFr;

    [SerializeField] private TMP_Text _temp1;
    [SerializeField] private TMP_Text _tempTarget1;
    [SerializeField] private TMP_Text _tempDopLabel11;
    [SerializeField] private TMP_Text _tempDopLabel12;
    [SerializeField] private MeasuringtTemp _measTemp1;

    [SerializeField] private TMP_Text _temp2;
    [SerializeField] private TMP_Text _tempTarget2;
    [SerializeField] private TMP_Text _tempDopLabel21;
    [SerializeField] private TMP_Text _tempDopLabel22;
    [SerializeField] private MeasuringtTemp _measTemp2;

    [SerializeField] private TMP_Text       _scenarioLabel;

    [SerializeField] private TMP_Text       _resText;

    [SerializeField] private WindowGraph    _GTrendGraph;
    [SerializeField] private WindowGraph    _IdTrendGraph;
    [SerializeField] private WindowGraph    _FsTrendGraph;
    [SerializeField] private WindowGraph    _YTrendGraph;

    [SerializeField] private WindowGraph    _XGraph;
    [SerializeField] private WindowGraph    _PGraph;
    [SerializeField] private WindowGraph    _TGraph;
    [SerializeField] private WindowGraph    _WorkDotGraph;
    [SerializeField] private TableManager   _XPTTabel;

    [SerializeField] private TableManager   _logTabel;

    [SerializeField] private TMP_Text       _resTextInstructor;

    [SerializeField] private WindowGraph    _GTrendGraphInstructor;
    [SerializeField] private WindowGraph    _IdTrendGraphInstructor;
    [SerializeField] private WindowGraph    _FsTrendGraphInstructor;
    [SerializeField] private WindowGraph    _YTrendGraphInstructor;

    [SerializeField] private WindowGraph    _XGraphInstructor;
    [SerializeField] private WindowGraph    _PGraphInstructor;
    [SerializeField] private WindowGraph    _TGraphInstructor;
    [SerializeField] private WindowGraph    _WorkDotGraphInstructor;

    [SerializeField] private TableManager   _logTabelInstructor;

    [SerializeField] private CollectData collectData;

    public string FilmName;
    [SerializeField] private CreateDuplicates extrudat;
    [SerializeField] private Image curColor;
    [SerializeField] private TMP_Text curColorDiff;
    [SerializeField] private WindowGraph delEGraph;
    [SerializeField] private WindowGraph delEGraphInstructor;

    private QPT_DIE_Adapter _qdAdapter =  new QPT_DIE_Adapter();

    void Start()
    {
        _entrPanel.OnChangeVal += EntrPanelOnChangeVal;

        /// Show G Id Fs Trend Graphs
        //_GTrendGraph .AddData(new Vector(0, 0));
        //_YTrendGraph .AddData(new Vector(0, 0));
        //_IdTrendGraph.AddData(new Vector(0, 0));
        //_FsTrendGraph.AddData(new Vector(0, 0));
    }

    public void SetInitData()
    {
        Clear();

        _qdAdapter.initData = collectData.GetInitData();
        InitData initData = _qdAdapter.initData;
        Train train = initData.train;

        _scenarioLabel.text = $"{train.G0:f2}\n" +
                              $"{train.Id_max:f2}\n" +
                              $"{train.Fs_max:f2}\n" +
                              $"{train.Is0:f2}\n";

        _operTime.SetTime(TimeSpan.FromMinutes(train.Time));

        _GTrendGraph.SetYMaxLine((float)train.G0);
        _YTrendGraph.SetYMaxLine((float)train.Is0);
        _IdTrendGraph.SetYMaxLine((float)train.Id_max);
        _FsTrendGraph.SetYMaxLine((float)train.Fs_max);

        _GTrendGraphInstructor.SetYMaxLine((float)train.G0);
        _YTrendGraphInstructor.SetYMaxLine((float)train.Is0);
        _IdTrendGraphInstructor.SetYMaxLine((float)train.Id_max);
        _FsTrendGraphInstructor.SetYMaxLine((float)train.Fs_max);

        _shnekSpeed.text = initData.data.N_.ToString();
        RecalcShnekSpeed();
        _temp1.text = initData.cyl[0].T_W_k_.ToString();
        SetTempTarget1();
        _temp2.text = initData.cyl[1].T_W_k_.ToString();
        SetTempTarget2();

        RecalcOutData();
    }

    void Clear()
    {
        /// Show res
        _resText.text = $"{0:f2}\n" +
                        $"{0:f2}\n" +
                        $"{0:f2}\n" +
                        $"{0:f2}\n";
        _resTextInstructor.text = $"{0:f2}\n" +
                                  $"{0d:f2}\n" +
                                  $"{0:f2}\n" +
                                  $"{0:f2}\n" +
                                  $"{0:f2}\n";

        //Обучаемый
        _GTrendGraph.Clear();
        _IdTrendGraph.Clear();
        _FsTrendGraph.Clear();
        _YTrendGraph.Clear();

        _XGraph.Clear();
        _PGraph.Clear();
        _TGraph.Clear();
        _WorkDotGraph.Clear();
        _XPTTabel.Clear();

        _logTabel.Clear();

        delEGraph.Clear();

        //Инструктор
        _GTrendGraphInstructor.Clear();
        _IdTrendGraphInstructor.Clear();
        _FsTrendGraphInstructor.Clear();
        _YTrendGraphInstructor.Clear();

        _XGraphInstructor.Clear();
        _PGraphInstructor.Clear();
        _TGraphInstructor.Clear();
        _WorkDotGraphInstructor.Clear();

        _logTabelInstructor.Clear();

        delEGraphInstructor.Clear();
    }

    void EntrPanelOnChangeVal(TMP_Text valText)
    {
        if (valText == _shnekSpeed)
        {
            RecalcShnekSpeed();
            RecalcOutData();
        }
        else if (valText == _voronkaSpeed)
        {
            _voronkaSpeedTarget.text = _voronkaSpeed.text;
            RecalcVoronkaFr();
        }
        else if (valText == _voronkaFr)
        {
            RecalcVoronkaSpeed();
        }
        else if (valText == _knifeSpeed)
        {
            _knifeSpeedTarget.text = _knifeSpeed.text;
            RecalcKnifeFr();
        }
        else if (valText == _knifeFr)
        {
            RecalcKnifeSpeed();
        }
        else if (valText == _temp1)
        {
            SetTempTarget1();
            RecalcOutData();
        }
        else if (valText == _temp2)
        {
            SetTempTarget2();
            RecalcOutData();
        }
    }

    //////////////// RECALC OUT DATA ////////////////
    void RecalcOutData()
    {
        RecalcOutDataAsync();
        //Task.Run(() => RecalcOutDataCoroutine());
    }

    async void RecalcOutDataAsync()
    {
        _qdAdapter.initData.data.N_       = Math.Max(double.Parse(_shnekSpeed.text), 0.1);
        _qdAdapter.initData.cyl[0].T_W_k_ = double.Parse(_temp1.text);
        _qdAdapter.initData.cyl[1].T_W_k_ = double.Parse(_temp2.text);
        await Task.Run(() => _qdAdapter.init());
        
        //await Task.Run(() =>
        //{
        //    string text = "";
        //    ///////////////////////////////////////////
        //    double n = _qdAdapter.initData.data.N_;
        //    for (int i = 0; i < 20; ++i)
        //    {
        //        _qdAdapter.initData.data.N_ = n + i;
        //        _qdAdapter.init();
        //        text += $"{G()}\n";
        //    }
        //    Debug.Log(text);

        //    double t = _qdAdapter.initData.cyl[1].T_W_k_;
        //    text = "";
        //    string ns = "";
        //    string ts = "";
        //    //for (int i = 0; i < 20; ++i)
        //    //{
        //    //    _qdAdapter.initData.data.N_ = n + i;
        //    //    for (int j = 0; j < 20; ++j)
        //    //    {
        //    //        _qdAdapter.initData.cyl[1].T_W_k_ = t + j;
        //    //        _qdAdapter.init();
        //    //        text += $"{Id()}\n";
        //    //        ns += $"{_qdAdapter.initData.data.N_}\n";
        //    //        ts += $"{_qdAdapter.initData.cyl[1].T_W_k_}\n";
        //    //    }
        //    //}
        //    //Debug.Log(text);
        //    //Debug.Log(ns);
        //    //Debug.Log(ts);

        //    for (int j = 0; j < 1; ++j)
        //    {
        //        for (int i = 0; i < 1; ++i)
        //        {
        //            _qdAdapter.initData.data.N_ = n + i;
        //            _qdAdapter.initData.cyl[1].T_W_k_ = t + j;
        //            _qdAdapter.init();
        //            text += $"{Id()}\n";
        //        }

        //        Debug.Log(text);
        //        text = "";
        //    }
        //});

        ///////////////////////////////////////////

        Train train = _qdAdapter.initData.train;
        float g = G();
        //float dG = g / (float)train.G_max * 100 - 100;
        //if (dG <= 0) dG = Math.Min(g / (float)train.G_min * 100 - 100, 0);
        float dG = g / (float)Math.Max(train.G0, 1e-5) * 100 - 100;
        float id = Id();

        (var color, var diff) = GetColorAndDiffFromBaseById(id);
        extrudat.extrudatColor = color;
        curColor.color = color;
        curColorDiff.text = $"{diff:f2}";
        delEGraph.AddData(new Vector((float)_operTime.GetMin(), diff));
        delEGraphInstructor.AddData(new Vector((float)_operTime.GetMin(), diff));

        float dId = Math.Max(id / (float)train.Id_max * 100 - 100, 0);
        float fs = Fs();
        float dFs = Math.Max(fs / (float)train.Fs_max * 100 - 100, 0);
        float y = Dmix();
        float dY = y / (float)Math.Max(train.Is0, 1e-5) * 100 - 100;

        /// Show res
        _resText.text = $"{g:f2}\n" +
                        $"{id:f2}\n" +
                        $"{fs:f2}\n" +
                        $"{y:f2}\n";
        _resTextInstructor.text = $"{g:f2}\n" +
                                  $"{id:f2}\n" +
                                  $"{fs:f2}\n" +
                                  $"{y:f2}\n" +
                                  $"{diff:f2}";

        /// Show X P T WorkDot Graphs
        _XGraph.SetData(_qdAdapter.qpt.XZ.Last());
        _PGraph.SetData(_qdAdapter.qpt.PZ.Last());
        _TGraph.SetData(_qdAdapter.qpt.TZ.Last());

        _XGraphInstructor.SetData(_qdAdapter.qpt.XZ.Last());
        _PGraphInstructor.SetData(_qdAdapter.qpt.PZ.Last());
        _TGraphInstructor.SetData(_qdAdapter.qpt.TZ.Last());

        List<Vector> mt = new List<Vector>();
        List<Vector> tout = new List<Vector>();
        for (int i = 0; i <= _qdAdapter.die.Res.n_Q; i++)
        {
            mt.Add(  new((float)(_qdAdapter.die.M_Q[i] * 1e2), (float)_qdAdapter.die.Res.MP[i]));
            tout.Add(new((float)(_qdAdapter.die.M_Q[i] * 1e2), (float)_qdAdapter.die.M_P[i]));
        }
        _WorkDotGraph.SetData(mt, tout);
        _WorkDotGraphInstructor.SetData(mt, tout);

        /// Show X P T Table
        _XPTTabel.SetData(_qdAdapter.qpt.ZXPT.Last());

        /// Show G Id Fs Y Trend Graphs
        _GTrendGraph .AddData(new Vector((float)_operTime.GetMin(), g));
        _IdTrendGraph.AddData(new Vector((float)_operTime.GetMin(), id));
        _FsTrendGraph.AddData(new Vector((float)_operTime.GetMin(), fs));
        _YTrendGraph .AddData(new Vector((float)_operTime.GetMin(), y));

        _GTrendGraphInstructor .AddData(new Vector((float)_operTime.GetMin(), g));
        _IdTrendGraphInstructor.AddData(new Vector((float)_operTime.GetMin(), id));
        _FsTrendGraphInstructor.AddData(new Vector((float)_operTime.GetMin(), fs));
        _YTrendGraphInstructor .AddData(new Vector((float)_operTime.GetMin(), y));

        /// Log 
        _logTabel.AddData(new List<object>
        {
            _operTime.GetTimeStr(),
            _qdAdapter.initData.data.N_,
            _qdAdapter.initData.cyl[0].T_W_k_,
            _qdAdapter.initData.cyl[1].T_W_k_,
            g,
            dG,
            id,
            dId,
            diff,
            fs,
            dFs,
            y,
            dY
        });

        _logTabelInstructor.AddData(new List<object>
        {
            _operTime.GetTimeStr(),
            _qdAdapter.initData.data.N_,
            _qdAdapter.initData.cyl[0].T_W_k_,
            _qdAdapter.initData.cyl[1].T_W_k_,
            g,
            dG,
            id,
            dId,
            diff,
            fs,
            dFs,
            y,
            dY
        });
    }

    private float G()
    {
        return (float)(_qdAdapter.die.RES_f.Q_fin * (_qdAdapter.qpt.DataRec.Ro_* 3600 * 1e-6));
    }
    
    private float Id()
    {
        const double T_kel = 273.15;
        const double R = 8.31;

        //const double t_d = 600;
        //const double E_d = 165000;
        //const double T_d = 220 + T_kel;
        const double t_d = 300;
        const double E_d = 80000;
        const double T_d = 200 + T_kel;

        double T_ext = _qdAdapter.qpt.T + T_kel;
        return (float)(t_av() / t_d * Math.Exp(E_d / (R * T_ext * T_d) * (T_ext - T_d)) * 100);
    }

    private float Fs()
    {
        double Q = _qdAdapter.die.RES_f.Q_fin * 1e3;
        double Q_s = _qdAdapter.initData.sect.Last().H_fin * _qdAdapter.qpt.X_PL * 1e3 * _qdAdapter.qpt.v_SZ * 1e3;

        if (Q == 0) return 0;

        return (float)(Q_s / Q);
    }

    private float Dmix()
    {
        return (float)(_qdAdapter.qpt.Sav * t_av());
    }

    private double t_av()
    {
        double V = 0;
        foreach (var sect in _qdAdapter.initData.sect)
        {
            if (sect.S_Type == 1)
            {
                double z = sect.L_sect / _qdAdapter.qpt.sn;
                V += z * _qdAdapter.qpt.W * 1e3 * ((sect.H_st + sect.H_fin) / 2);
            }
            else
            {
                double D = (sect.D_st + sect.D_fin) / 2;
                double d = sect.D_st - 2 * sect.H_st;
                V += 0.25 * Math.PI * sect.L_sect * (D*D - d*d);
            }
        }

        return V / (_qdAdapter.die.RES_f.Q_fin * 1e3);
    }

    //////////////// RECALC PARAMS ON CONTROL PANEL //////////////// 
    void RecalcShnekSpeed()
    {
        _shnekSpeedTarget.text = _shnekSpeed.text;

        RecalcVoronkaSpeed();
        RecalcKnifeSpeed();
    }

    void RecalcVoronkaSpeed()
    {
        double N = double.Parse(_shnekSpeed.text);
        double fr = double.Parse(_voronkaFr.text);
        _voronkaSpeed.text = string.Format("{0:f}", (1 + fr / 100) * N);
        _voronkaSpeedTarget.text = _voronkaSpeed.text;
    }

    void RecalcVoronkaFr()
    {
        double N = double.Parse(_shnekSpeed.text);
        double Nh = double.Parse(_voronkaSpeed.text);
        _voronkaFr.text = string.Format("{0:f}", (Nh / N - 1) * 100);
    }

    void RecalcKnifeSpeed()
    {
        double N = double.Parse(_shnekSpeed.text);
        double fr = double.Parse(_knifeFr.text);
        _knifeSpeed.text = string.Format("{0:f}", (1 + fr / 100) * N);
        _knifeSpeedTarget.text = _knifeSpeed.text;
    }

    void RecalcKnifeFr()
    {
        double N = double.Parse(_shnekSpeed.text);
        double Nh = double.Parse(_knifeSpeed.text);
        _knifeFr.text = string.Format("{0:f}", (Nh / N - 1) * 100);
    }

    void SetTempTarget1()
    {
        _tempTarget1.text = _temp1.text;
        _tempDopLabel11.text = _temp1.text;
        _tempDopLabel12.text = _temp1.text;
        _measTemp1.SetValue(_temp1.text);
    }

    void SetTempTarget2()
    {
        _tempTarget2.text = _temp2.text;
        _tempDopLabel21.text = _temp2.text;
        _tempDopLabel22.text = _temp2.text;
        _measTemp2.SetValue(_temp2.text);
    }

    private List<ColorInterval> GetFilmIntervals()
    {
        using var ctx = new ExtrusionContext();
        return ctx.ColorIntervals.Include(ci => ci.Film).Where(ci => ci.Film.Type == FilmName).AsNoTracking().ToList();
    }

    private (Color, float) GetColorAndDiffFromBaseById(float Id)
    {
        var intervals = GetFilmIntervals();
        var curInter = intervals.FirstOrDefault(i => i.MaxDelE > Id && i.MinDelE <= Id);
        if (curInter is null)
        {
            Debug.Log("No such interval");
            return (Color.black, -1);
        }
        var curColor = new Vector3(curInter.L, curInter.a, curInter.b);
        var baseInterval = intervals.FirstOrDefault(i => i.IsBaseColor);
        var baseColor = new Vector3(baseInterval.L, baseInterval.a, baseInterval.b);

        return (ColorHelper.LabToRGB(curColor), ColorHelper.GetLabColorsDiff(baseColor, curColor));
    }
}
