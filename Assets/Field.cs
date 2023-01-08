using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static FieldType;

public class Field : MonoBehaviour
{
    private static Color harvestedColor = new Color(0.627f, 0.471f, 0.357f, 1.000f);

    public Sprite Seed;
    public Color SeedColor;
    public Sprite PlantSeed;
    public Color PlantColor;
    public Sprite Sprout;
    public Color SproutColor;
    public Sprite Corn;
    public Color CornColor;

    public FieldType FieldType { get; private set; } = new FieldType(GrowState.Harvested, harvestedColor, null);

    public bool IsDestroyed;

    public GrowState state;

    private SpriteRenderer growIndicatorSprite;
    private SpriteRenderer fieldSprite;

    private void Awake()
    {
        fieldSprite = GetComponent<SpriteRenderer>();
        growIndicatorSprite = GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "GrowIndicator");
        growIndicatorSprite.enabled = false;
        Set(state);
    }

    public void Set(GrowState growState)
    {
        if (IsDestroyed)
        {
            return;
        }
        Color color = harvestedColor;
        Sprite sprite = null;
        switch (growState)
        {
            case GrowState.PlantSeed:
                color = PlantColor;
                sprite= PlantSeed;
                break;
            case GrowState.Sprout:
                color = SproutColor;
                sprite = Sprout;
                break;
            case GrowState.Corn:
                color = CornColor;
                sprite = Corn;
                break;
            case GrowState.Seed:
                color = SeedColor;
                sprite = Seed;
                break;
        }
        FieldType = new FieldType(growState, color, sprite);
        if (growIndicatorSprite != null)
        {
            growIndicatorSprite.sprite = sprite;
            growIndicatorSprite.size = new Vector2(10, 5);
            growIndicatorSprite.enabled = growState != GrowState.Harvested;
        }
        if (fieldSprite != null)
        {
            fieldSprite.color = color;
        }
    }

    public enum GrowChance
    {
        Normal,
        Good,
        VerGood,
        Bad,
        VeryBad,
        NotLikly,
    }

    public int Grow(GrowChance growChance)
    {
        if (IsDestroyed)
        {
            return 0;
        }
        var growChanceValue = 10;
        switch (growChance)
        {
            case GrowChance.Good:
                growChanceValue = 7;
                break;
            case GrowChance.VerGood:
                growChanceValue = 3;
                break;
            case GrowChance.Bad:
                growChanceValue = 12;
                break;
            case GrowChance.VeryBad:
                growChanceValue = 16;
                break;
            case GrowChance.NotLikly:
                growChanceValue = 19;
                break;
        }
        if (UnityEngine.Random.Range(0, 21) < growChanceValue)
        {
            // ignore grow cycle
            return 0;
        }
        switch (FieldType.Grow)
        {
            case GrowState.PlantSeed:
                Set(GrowState.Sprout);
                break;
            case GrowState.Sprout:
                Set(GrowState.Corn);
                break;
            case GrowState.Corn:
                Set(GrowState.Harvested);
                return 1;
            case GrowState.Seed:
                Set(GrowState.PlantSeed);
                break;
            case GrowState.Harvested:
                Set(GrowState.Seed);
                break;
        }
        return 0;
    }
}
