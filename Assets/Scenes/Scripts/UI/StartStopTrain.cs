using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Types;
using UnityEngine;
using UnityEngine.UI;

public class StartStopTrain : MonoBehaviour
{
    [SerializeField] private GameObject _instr;
    [SerializeField] private VrButton _stopBtn;
    [SerializeField] private MouseButton _click;
    [SerializeField] private Button _startBtn;
    [SerializeField] private CurrTimeUpdate _leftTime;

    [SerializeField] private Recalculates calcs;
    [SerializeField] private TMP_Dropdown filmDD;
    private bool _enabled = true;

    void Start()
    {
        _stopBtn.down.AddListener(StopTrain);
        _click.click.AddListener(StopTrain);
        _startBtn.onClick.AddListener(StartTrain);
    }
    
    public void SetEnabled(bool enabled) => _enabled = enabled;

    void Update()
    {
        if (_enabled && _leftTime.GetLeftMin() <= 0)
        {
            StopTrain();
        }
    }

    public void StartTrain()
    {
        calcs.FilmName = filmDD.captionText.text;
        _instr.SetActive(false);
    }

    public void StopTrain()
    {
        _instr.SetActive(true);
        _leftTime.SetTime(TimeSpan.FromMinutes(0));
    }
}