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

        _resText.text = string.Format("Показатели качества\n" +
                                      "G =  {0:f2} кг/ч - производительность\n" +
                                      "Id = {1:f2} % - индекс деструкции\n" +
                                      "Фs = {2:f2} - доля нерасплавленных включений\n" +
                                      "Y =  {3:f2} - степень смешения",                    
                                      G(),
                                      Id(),
                                      fs(),
                                      0);
        
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
                _qdAdapter.qpt.XZ.Last()[i].X,
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
            (double)_qdAdapter.qpt.XZ.Last().Last().Y
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
                V += z * _qdAdapter.qpt.W * ((sect.H_st + sect.H_fin) / 2);
            }
            else
            {
                double d = sect.D_st - 2 * sect.H_st;
                V += 0.25 * Math.PI * sect.L_sect * (sect.D_st * sect.D_st - d*d);
            }
        }

        double t_av = V / (_qdAdapter.die.RES_f.Q_fin * 1e6);

        const double T_kel = 273.15;
        const double R = 8.31;

        // t_d = 600 с, T_d = 220 град. С, E_d = 165000 Дж/моль.
        const double t_d = 600;
        const double E_d = 165000;
        const double T_d = 220 + T_kel;

        double T_ext = _qdAdapter.qpt.T + T_kel;

        return t_av / t_d * Math.Exp(E_d / (R * T_ext * T_d) * (T_ext - T_d)) * 100;
    }

    private double fs()
    {
        double Q = _qdAdapter.initData.dop.Q;
        double Q_s = _qdAdapter.initData.sect.Last().H_fin * _qdAdapter.qpt.X_PL * _qdAdapter.qpt.v_SZ;
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
