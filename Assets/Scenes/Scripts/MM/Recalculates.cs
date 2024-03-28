using Extruder;
using Program;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Recalculates : MonoBehaviour
{
    [SerializeField] private EntryValuePanel _entrPanel;

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
    private List<System.Numerics.Vector2> _GTrends;

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
    }

    public void SetInitData()
    {
        _qdAdapter.initData = collectData.GetInitData();

        _shnekSpeed.text = _qdAdapter.initData.data.N_.ToString();
        RecalcShnekSpeed();
        _temp1.text = _qdAdapter.initData.cyl[0].T_W_k_.ToString();
        SetTempTarget1();
        _temp2.text = _qdAdapter.initData.cyl[1].T_W_k_.ToString();
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
        _qdAdapter.initData.data.N_ =       double.Parse(_shnekSpeed.text);
        _qdAdapter.initData.cyl[0].T_W_k_ = double.Parse(_temp1.text);
        _qdAdapter.initData.cyl[1].T_W_k_ = double.Parse(_temp2.text);
        _qdAdapter.init();

        _resText.text = string.Format("Выходные параметры процесса экструзии:\n" +
                                      "Производительность - {0:f2} кг/ч\n" +
                                      "Индекс термической деструкции экструдате - {1:f2} %\n" +
                                      "Доля твердой фазы в экструдате - {2:f2}\n",          
                                      G(),
                                      Id(),
                                      fs());
        
        /// Show X P T Graphs
        _XGraph.Show(_qdAdapter.qpt.XZ.Last());
        _PGraph.Show(_qdAdapter.qpt.PZ.Last());
        _TGraph.Show(_qdAdapter.qpt.TZ.Last());
        
        ///Show X P T Table
        List<List<double>> rows = new List<List<double>>();
        for (int i = 0; i < _qdAdapter.qpt.XZ.Last().Count(); ++i)
        {
            rows.Add(new List<double>()
            {
                _qdAdapter.qpt.PZ.Last()[i].X,
                _qdAdapter.qpt.XZ.Last()[i].Y,
                _qdAdapter.qpt.PZ.Last()[i].Y,
                _qdAdapter.qpt.TZ.Last()[i].Y,
            });
        }

        _XPTTabel.SetData(rows);

        ///Log 
        List<object> data = new List<object>
        {
            System.DateTime.Now.ToString("HH:mm:ss"),
            double.Parse(_shnekSpeed.text),
            G(),
            Id(),
            fs()
        };
        _logTabel.AddData(data);
    }

    private double G()
    {
        return _qdAdapter.die.RES_f.Q_fin * (_qdAdapter.qpt.DataRec.Ro_* 3600 * 1e-6);
    }
     
    private double Id()
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
        return t_av / t_d * Math.Exp(E_d / (R * T_ext * T_d) * (T_ext - T_d)) * 100;
    }

    private double fs()
    {
        double Q = _qdAdapter.die.RES_f.Q_fin * 1e3;
        double Q_s = _qdAdapter.initData.sect.Last().H_fin * _qdAdapter.qpt.X_PL * 1e3 * _qdAdapter.qpt.v_SZ * 1e3;

        if (Q == 0) return 0;

        return Q_s / Q;
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
