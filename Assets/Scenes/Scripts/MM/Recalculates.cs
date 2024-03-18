using Extruder;
using Program;
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

    [SerializeField] private TMP_Text _resText;
    [SerializeField] private WindowGraph _XGraph;
    [SerializeField] private WindowGraph _PGraph;
    [SerializeField] private WindowGraph _TGraph;
    [SerializeField] private TableManager _XPTTabel;

    [SerializeField] private TableManager _logTabel;

    private InitData initData = new InitData();
    private QPT_DIE_Adapter _qdAdapter =  new QPT_DIE_Adapter();

    void Start()
    {
        _entrPanel.OnChangeVal += EntrPanelOnChangeVal;

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
    }

    //////////////// RECALC OUT DATA ////////////////
    void RecalcOutData()
    {
        initData.data.N_ = double.Parse(_shnekSpeed.text);
        _qdAdapter.init(initData);

        _resText.text = string.Format("Показатели качества\n" +
                                      "G =  {0:f2} кг/ч - производительность\n" +
                                      "Id = {1:f2} % - индекс деструкции\n" +
                                      "Фs = {2:f2} % - доля нерасплавленных включений\n" +
                                      "Y =  {3:f2} - степень смешения",                    
                                      _qdAdapter.die.RES_f.Q_fin * (_qdAdapter.qpt.DataRec.Ro_ * 3600 * 1e-6),
                                      0,
                                      0,
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
}
