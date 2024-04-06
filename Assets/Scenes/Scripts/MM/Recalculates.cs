using Extruder;
using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Types;
using UnityEngine;
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
    [SerializeField] private TMP_Text _temp2;
    [SerializeField] private TMP_Text _tempTarget2;

    [SerializeField] private TMP_Text _resText;

    [SerializeField] private WindowGraph _GTrendGraph;
    [SerializeField] private WindowGraph _IdTrendGraph;
    [SerializeField] private WindowGraph _FsTrendGraph;

    [SerializeField] private WindowGraph _XGraph;
    [SerializeField] private WindowGraph _PGraph;
    [SerializeField] private WindowGraph _TGraph;
    [SerializeField] private TableManager _XPTTabel;

    [SerializeField] private TableManager _logTabel;

    [SerializeField] private CollectData collectData;

    private QPT_DIE_Adapter _qdAdapter =  new QPT_DIE_Adapter();

    void Start()
    {
        _entrPanel.OnChangeVal += EntrPanelOnChangeVal;

        /// Show G Id Fs Trend Graphs
        _GTrendGraph .AddData(new Vector(0, 0));
        _IdTrendGraph.AddData(new Vector(0, 0));
        _FsTrendGraph.AddData(new Vector(0, 0));
    }

    public void SetInitData()
    {
        _qdAdapter.initData = collectData.GetInitData();
        InitData initData = _qdAdapter.initData;
        Train train = initData.train;

        _operTime.trainTime = TimeSpan.FromMinutes(train.Time);

        _GTrendGraph.SetYMaxMinLine((float)train.G_max, (float)train.G_min);
        _IdTrendGraph.SetYMaxLine((float)train.Id_max);
        _FsTrendGraph.SetYMaxLine((float)train.Fs_max);

        _shnekSpeed.text = initData.data.N_.ToString();
        RecalcShnekSpeed();
        _temp1.text = initData.cyl[0].T_W_k_.ToString();
        SetTempTarget1();
        _temp2.text = initData.cyl[1].T_W_k_.ToString();
        SetTempTarget2();

        RecalcOutData();
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
        _qdAdapter.initData.data.N_       = Math.Max(double.Parse(_shnekSpeed.text), 0.1);
        _qdAdapter.initData.cyl[0].T_W_k_ = double.Parse(_temp1.text);
        _qdAdapter.initData.cyl[1].T_W_k_ = double.Parse(_temp2.text);
        _qdAdapter.init();

        Train train = _qdAdapter.initData.train;
        float g = G();
        float dG = g / (float)train.G_max * 100 - 100;
        if (dG <= 0) dG = Math.Min(g / (float)train.G_min * 100 - 100, 0);
        float id = Id();
        float dId = Math.Max(id / (float)train.Id_max * 100 - 100, 0);
        float fs = Fs();
        float dFs = Math.Max(fs / (float)train.Fs_max * 100 - 100, 0);

        /// Show res
        _resText.text = $"{g:f2}\n" +
                        $"{id:f2}\n" +
                        $"{fs:f2}\n";
        
        /// Show X P T Graphs
        _XGraph.SetData(_qdAdapter.qpt.XZ.Last());
        _PGraph.SetData(_qdAdapter.qpt.PZ.Last());
        _TGraph.SetData(_qdAdapter.qpt.TZ.Last());
        
        /// Show X P T Table
        _XPTTabel.SetData(_qdAdapter.qpt.ZXPT.Last());

        /// Show G Id Fs Trend Graphs
        _GTrendGraph .AddData(new Vector((float)_operTime.GetMin(), g));
        _IdTrendGraph.AddData(new Vector((float)_operTime.GetMin(), id));
        _FsTrendGraph.AddData(new Vector((float)_operTime.GetMin(), fs));

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
            fs,
            dFs
        });
    }

    private float G()
    {
        return (float)(_qdAdapter.die.RES_f.Q_fin * (_qdAdapter.qpt.DataRec.Ro_* 3600 * 1e-6));
    }
     
    private float Id()
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
                double d = sect.D_st - 2 * sect.H_st;
                V += 0.25 * Math.PI * sect.L_sect * (sect.D_st * sect.D_st - d*d);
            }
        }

        double t_av = V / (_qdAdapter.die.RES_f.Q_fin * 1e3);

        const double T_kel = 273.15;
        const double R = 8.31;

        //const double t_d = 600;
        //const double E_d = 165000;
        //const double T_d = 220 + T_kel;
        const double t_d = 300;
        const double E_d = 80000;
        const double T_d = 200 + T_kel;

        double T_ext = _qdAdapter.qpt.T + T_kel;
        return (float)(t_av / t_d * Math.Exp(E_d / (R * T_ext * T_d) * (T_ext - T_d)) * 100);
    }

    private float Fs()
    {
        double Q = _qdAdapter.die.RES_f.Q_fin * 1e3;
        double Q_s = _qdAdapter.initData.sect.Last().H_fin * _qdAdapter.qpt.X_PL * 1e3 * _qdAdapter.qpt.v_SZ * 1e3;

        if (Q == 0) return 0;

        return (float)(Q_s / Q);
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
    }

    void SetTempTarget2()
    {
        _tempTarget2.text = _temp2.text;
    }
}
