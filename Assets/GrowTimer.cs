using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GrowTimer : MonoBehaviour
{
    public Slider growProgress;
    public TextMeshProUGUI growText;
    public float CurrentTime = 0;
    public bool IsRunning = false;
    public float TimeDivided = 10;
    public float TimeDividedFromGrowChance = 0;

    public Color NormalGrowChanceColor;
    public Color GoodGrowChanceColor;
    public Color VerGoodGrowChanceColor;
    public Color BadGrowChanceColor;
    public Color VeryBadGrowChanceColor;
    public Color NotLiklyGrowChanceColor;

    public UnityEvent ReachedNewCycle;

    private void Start()
    {
        IsRunning = true;
    }

    private void Update()
    {
        if (IsRunning)
        {
            if (CurrentTime < 1f)
            {
                var value = TimeDivided - TimeDividedFromGrowChance;
                if (value < 0.01f)
                {
                    value = 0.01f;
                }
                CurrentTime += Time.deltaTime / value;
            }
            else
            {
                CurrentTime = 1f;
                growProgress.value = 1f;
                ReachedNewCycle.Invoke();
            }
            growProgress.value = CurrentTime;
        }
    }

    public void AddValue(float value)
    {
        CurrentTime += value;
    }

    public void ResetTimer()
    {
        CurrentTime = 0f;
        growProgress.value = 0f;
    }

    public void SiloEmpty()
    {
        IsRunning = false;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void RateChange(Field.GrowChance growChance)
    {
        string rateText = "";
        switch (growChance)
        {
            case Field.GrowChance.Normal:
                TimeDividedFromGrowChance = 0;
                break;
            case Field.GrowChance.Good:
                TimeDividedFromGrowChance = 5;
                rateText = "GOOD";
                break;
            case Field.GrowChance.VerGood:
                TimeDividedFromGrowChance = 9;
                rateText = "VERY GOOD";
                break;
            case Field.GrowChance.Bad:
                TimeDividedFromGrowChance = -5;
                rateText = "BAD";
                break;
            case Field.GrowChance.NotLikly:
            case Field.GrowChance.VeryBad:
                TimeDividedFromGrowChance = -10;
                rateText = "VERY BAD";
                break;
        }
        if (!string.IsNullOrEmpty(rateText))
        {
            growText.text = $"Next Growcycle [{rateText}]";
        }
        else
        {
            growText.text = "Next Growcycle";
        }
    }
}
