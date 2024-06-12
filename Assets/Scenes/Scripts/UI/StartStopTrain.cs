using System;
using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;
using UnityEngine.UI;

public class StartStopTrain : MonoBehaviour
{
    [SerializeField] private GameObject _zatichka;
    [SerializeField] private VrButton _stopBtn;
    [SerializeField] private Button _startBtn;
    [SerializeField] private CurrTimeUpdate _leftTime;

    void Start()
    {
        _stopBtn.down.AddListener(StopTrain);
        _startBtn.onClick.AddListener(StartTrain);
    }

    void Update()
    {
        if (_leftTime.GetLeftMin() <= 0)
        {
            StopTrain();
        }
    }

    public void StartTrain()
    {
        _zatichka.SetActive(false);
    }

    public void StopTrain()
    {
        _zatichka.SetActive(true);
        _leftTime.SetTime(TimeSpan.FromMinutes(0));    
    }
}