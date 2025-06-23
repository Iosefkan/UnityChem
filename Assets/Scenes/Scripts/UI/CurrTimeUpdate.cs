using System;
using TMPro;
using UnityEngine;

public class CurrTimeUpdate : MonoBehaviour
{
    TimeSpan? trainTime;

    TMP_Text _text;
    DateTime _startTime;

    private void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
        _startTime = DateTime.Now;
    }

    void Update()
    {
        if (trainTime is null) return;
        _text.text = GetTimeStr();
    }

    public void SetTime(TimeSpan time)
    {
        trainTime = time;
        _startTime = DateTime.Now;
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
        if (trainTime.HasValue) return trainTime.Value - GetTime();
        else return TimeSpan.Zero;
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
