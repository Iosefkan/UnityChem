using System;
using TMPro;
using UnityEngine;

public class CurrTimeUpdate : MonoBehaviour
{
    public TimeSpan trainTime;

    TMP_Text _text;
    DateTime _startTime;

    private void Start()
    {
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
        _startTime = DateTime.Now;
    }

    void Update()
    {
        _text.text = GetTimeStr();
    }

    public TimeSpan GetTime()
    {
        return DateTime.Now - _startTime;
    }

    public double GetMin()
    {
        return GetTime().TotalMinutes;
    }

    public string GetTimeStr()
    {
        TimeSpan operTime = GetTime();
        return $"{(int)operTime.TotalMinutes}:{operTime.Seconds:00}";
    }

    public TimeSpan GetLeftTime()
    {
        return trainTime - GetTime();
    }

    public double GetLeftMin()
    {
        return GetLeftTime().TotalMinutes;
    }

    public string GetLeftTimeStr()
    {
        TimeSpan time = GetLeftTime();
        return $"{(int)time.TotalMinutes}:{time.Seconds:00}";
    }
}
