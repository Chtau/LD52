using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldType
{
	public enum GrowState
	{
        Harvested,
        Seed,
		PlantSeed,
		Sprout,
		Corn,
	}

	public GrowState Grow { get; }
	public Color CurrentColor { get; }
	public Sprite CurrentSprite { get; }
	public int GrowProgress { get; private set; }

	public FieldType(GrowState growState, Color currentColor, Sprite currentSprite)
	{
		Grow = growState;
		CurrentColor = currentColor;
		CurrentSprite = currentSprite;
		GrowProgress = 0;
    }

	public void UpdateProgress(int newValue)
	{
		GrowProgress = newValue;
	}
}
