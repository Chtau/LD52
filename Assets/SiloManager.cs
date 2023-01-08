using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SiloManager : MonoBehaviour
{
    public float CurrentTime = 10;
    public bool IsRunning = false;
    public float TimeDivider = 1f;
    public TextMeshProUGUI SiloText;
    public UnityEvent SiloEmpty;
    public SpriteRenderer FillSprite;
    public EventDisplay eventDisplay;

    public int SiloMaxValue = 50;
    public int SiloValue = 31;
    public int ConsumePerCycle = 1;

    public UnityEvent<int> AddFarmer;
    public UnityEvent<int> RemoveFarmer;

    private DateTime lastChangedFarmer = DateTime.UtcNow;
    private int dayMultiplyer;

    // Start is called before the first frame update
    void Start()
    {
        IsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            if (CurrentTime > 0)
            {
                CurrentTime -= Time.deltaTime / TimeDivider;
            }
            else
            {
                CurrentTime = 0;
                ConsumeSiloValue();
                ResetTime();
            }
        }
    }

    private void ResetTime()
    {
        CurrentTime = 10;
    }

    private void ConsumeSiloValue()
    {
        var rndChance = UnityEngine.Random.Range(0, 10);
        var consumeValue = ConsumePerCycle;
        if (rndChance < dayMultiplyer)
        {
            consumeValue = ConsumePerCycle * 2;
        }
        AddValue(-consumeValue);
        eventDisplay.FarmerMessage($"{consumeValue} Corn consumed");
    }

    private void UpdateTextValue()
    {
        SiloText.text = $"{SiloValue}/{SiloMaxValue}";
    }

    private void UpdateFillSprite()
    {
        var sY = 6.42f / SiloMaxValue;
        var pY = 3.22f / SiloMaxValue;

        FillSprite.transform.localScale = new Vector3(1.62f, -(sY * SiloValue), 1f);
        FillSprite.transform.localPosition = new Vector3(0f, -3.9f + (pY * SiloValue), 1f);
    }

    public void AddValue(int value)
    {
        SiloValue += value;
        if (SiloValue <= 0)
        {
            IsRunning = false;
            SiloValue = 0;
            SiloEmpty.Invoke();
        }
        else if (SiloValue < SiloMaxValue)
        {
            if (SiloValue > SiloMaxValue)
            {
                SiloValue = SiloMaxValue;
            }
            UpdateTextValue();
            UpdateFillSprite();
            
            if ((DateTime.UtcNow - lastChangedFarmer).TotalSeconds > 15)
            {
                if (SiloValue > 40)
                {
                    AddFarmer.Invoke(SiloValue);
                    lastChangedFarmer = DateTime.UtcNow;
                }
                else if (SiloValue < 20)
                {
                    RemoveFarmer.Invoke(SiloValue);
                    lastChangedFarmer = DateTime.UtcNow;
                }
            }
        }
    }

    public void FarmerCountChanged(int value)
    {
        ConsumePerCycle = value;
    }

    public void DayMultiplyer(int value)
    {
        dayMultiplyer = value;
    }
}
