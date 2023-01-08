using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ActionButton;

public class FarmesManager : MonoBehaviour
{
    private GrowTimer growTimer;

    public Slider sliderTemperature;
    public Slider sliderWater;
    public Slider sliderSun;

    public TextMeshProUGUI DaysCountText;
    public TextMeshProUGUI FarmesCountText;
    public int FarmesCount = 1;
    public FarmManager[] farms;
    public EventDisplay eventDisplay;
    public UnityEvent<int> FarmersCountChanged;
    public UnityEvent<int> DayChanged;

    private float temperatureValue = .5f;
    private float waterValue = .5f;
    private float sunValue = .5f;
    private bool gameover = false;
    private int maxFarms = 7;
    private int currentDay = 0;

    private void Awake()
    {
        UpdateDaysCount(currentDay);
        maxFarms = farms.Length;
        growTimer = GetComponent<GrowTimer>();
        FarmesCountUpdate(1);
    }

    private float secTime = 0;
    private void Update()
    {
        if (gameover)
            return;
        secTime += Time.deltaTime;

        if (secTime % 1f > 0.99)
        {
            RandomUpdateStats();
            secTime = 0;
        }
    }

    private void FarmesCountUpdate(int value)
    {
        FarmesCount = value;
        FarmesCountText.text = $"{value}x";
        FarmersCountChanged.Invoke(FarmesCount);
    }

    private bool nightStarted = false;

    public void WeatherEffect(int effect)
    {
        if (gameover)
            return;
        var weatherEffect = (WeatherEffect)effect;
        switch (weatherEffect)
        {
            case ActionButton.WeatherEffect.Sun:
                if (nightStarted)
                {
                    nightStarted = false;
                    currentDay += 1;
                    UpdateDaysCount(currentDay);
                }
                SetStatsValue(Random.Range(0.14f, 0.3f), -Random.Range(0.11f, 0.25f), Random.Range(0.09f, 0.3f));
                break;
            case ActionButton.WeatherEffect.Night:
                nightStarted = true;
                SetStatsValue(-Random.Range(0.14f, 0.3f), -Random.Range(0.11f, 0.25f), -Random.Range(0.09f, 0.3f));
                break;
            case ActionButton.WeatherEffect.Rain:
                SetStatsValue(-Random.Range(0.04f, 0.3f), Random.Range(0.05f, 0.2f), -Random.Range(0.02f, 0.2f));
                break;
            case ActionButton.WeatherEffect.Snow:
                SetStatsValue(-Random.Range(0.2f, 0.6f), Random.Range(0.1f, 0.3f), -Random.Range(0.02f, 0.35f));
                break;
            case ActionButton.WeatherEffect.Lightning:
                if (Random.Range(0, 10) > 4)
                {
                    SetStatsValue(-Random.Range(0.01f, 0.1f), -Random.Range(0.01f, 0.1f), -Random.Range(0.01f, 0.1f));
                }
                else
                {
                    SetStatsValue(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
                }
                break;
            case ActionButton.WeatherEffect.Whirlwind:
                if (Random.Range(0, 10) > 4)
                {
                    SetStatsValue(Random.Range(0.01f, 0.6f), Random.Range(0.01f, 0.6f), Random.Range(0.00f, 0.6f));
                }
                else
                {
                    SetStatsValue(-Random.Range(0.01f, 0.6f), -Random.Range(0.01f, 0.6f), Random.Range(0.00f, 0.6f));
                }
                break;
            case ActionButton.WeatherEffect.Cloudy:
                SetStatsValue(-Random.Range(0.01f, 0.05f), Random.Range(0.0f, 0.05f), -Random.Range(0.1f, 0.25f));
                break;
            case ActionButton.WeatherEffect.CloudsOpenUp:
                SetStatsValue(Random.Range(0.01f, 0.05f), -Random.Range(0.0f, 0.05f), Random.Range(0.1f, 0.25f));
                break;
            case ActionButton.WeatherEffect.Meteor:
                RemoveFarmer(0);
                break;
            default:
                break;
        }
        SetStats();
    }

    public void SiloEmpty()
    {
        GameOver();
    }

    public void NextGrowCycle()
    {
        if (gameover)
            return;
        var corn = 0;
        var chance = GetGrowChance();
        foreach (var farm in farms.Where(x => x.IsActive))
        {
            corn += farm.GrowCycle(chance);
        }
        growTimer.RateChange(chance);
        growTimer.ResetTimer();
        if (corn > 0)
        {
            eventDisplay.Message($"Harvested {corn} corn.");
        }
    }

    public void AddFarmer(int siloValue)
    {
        if (FarmesCount < maxFarms)
        {
            var farm = System.Array.Find(farms, x => !x.IsActive);
            if (farm != null)
            {
                farm.Active(true);
                FarmesCountUpdate(FarmesCount + 1);
            }
        }
    }

    public void RemoveFarmer(int siloValue)
    {
        if (FarmesCount > 0)
        {
            if (siloValue > 0 && FarmesCount == 1)
            {
                return;
            }
            var farm = System.Array.Find(farms, x => x.IsActive);
            if (farm != null)
            {
                farm.Active(false);
                FarmesCountUpdate(FarmesCount - 1);
            }
        }
        if (!gameover && FarmesCount == 0)
        {
            GameOver();
        }
    }

    private void SetStatsValue(float temp, float water, float sun)
    {
        bool failed = false;
        temperatureValue += temp;
        if (temperatureValue >= 1f)
        {
            temperatureValue = 1f;
            failed = true;
        }
        else if (temperatureValue <= 0f)
        {
            temperatureValue = 0f;
            failed = true;
        }
            
        waterValue += water;
        if (waterValue >= 1f)
        {
            waterValue = 1f;
            failed = true;
        }
        else if (waterValue <= 0f)
        {
            waterValue = 0f;
            failed = true;
        }

        sunValue += sun;
        if (sunValue >= 1f)
        {
            sunValue = 1f;
            failed = true;
        }
        else if (sunValue <= 0f)
        {
            sunValue = 0f;
            failed = true;
        }

        if (failed)
        {
            //GameOver();
        }
    }

    private void SetStats()
    {
        sliderTemperature.value = temperatureValue;
        sliderWater.value = waterValue;
        sliderSun.value = sunValue;
    }

    private Field.GrowChance GetGrowChance()
    {
        if (temperatureValue < .40f || waterValue < .40f || sunValue < .40f)
        {
            if ((temperatureValue + waterValue + sunValue) / 3 < .10f)
                return Field.GrowChance.NotLikly;
            if ((temperatureValue + waterValue + sunValue) / 3 < .25f)
                return Field.GrowChance.VeryBad;
            return Field.GrowChance.Bad;
        }
        if (temperatureValue > .75f || waterValue > .75f || sunValue > .75f)
        {
            if ((temperatureValue + waterValue + sunValue) / 3 > .9f)
                return Field.GrowChance.VeryBad;
            return Field.GrowChance.Bad;
        }
        if (temperatureValue > .5f && temperatureValue < .55f &&
            waterValue > .5f && waterValue < .55f &&
            sunValue > .5f && sunValue < .55f)
        {
            return Field.GrowChance.VerGood;
        }
        if (temperatureValue > .45f && temperatureValue < .65f &&
            waterValue > .45f && waterValue < .65f &&
            sunValue > .45f && sunValue < .65f)
        {
            return Field.GrowChance.Good;
        }
        return Field.GrowChance.Normal;
    }

    private void GameOver()
    {
        gameover = true;
        FarmesCountUpdate(0);
        growTimer.Stop();
        if (currentDay == 1)
        {
            eventDisplay.MessageDialog($"Game over after {currentDay} day.");
        }
        else
        {
            eventDisplay.MessageDialog($"Game over after {currentDay} days.");
        }
    }

    private void UpdateDaysCount(int value)
    {
        DaysCountText.text = $"Day {value}";
        DayChanged.Invoke(value);
    }

    private void RandomUpdateStats()
    {
        var tempValue = 0f;
        var tempChance = temperatureValue > .6f || temperatureValue < .4f ? 4 : 6;
        if (Random.Range(0, 10) > tempChance)
        {
            if (temperatureValue < .5f)
                tempValue = -Random.Range(.0f, .02f);
            else
                tempValue = Random.Range(.0f, .02f);
        }
        var waValue = 0f;
        var waterChance = waterValue > .6f || waterValue < .4f ? 4 : 6;
        if (Random.Range(0, 10) > waterChance)
        {
            if (waterValue < .5f)
                waValue = -Random.Range(.0f, .02f);
            else
                waValue = Random.Range(.0f, .02f);
        }
        var suValue = 0f;
        var sunChance = sunValue > .6f || sunValue < .4f ? 4 : 6;
        if (Random.Range(0, 10) > sunChance)
        {
            if (sunValue < .5f)
                suValue = -Random.Range(.0f, .02f);
            else
                suValue = Random.Range(.0f, .02f);
        }

        SetStatsValue(tempValue, waValue, suValue);
        SetStats();
    }
}
