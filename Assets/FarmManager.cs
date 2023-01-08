using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    private Field[] fields;

    public UnityEvent<int> CornHarvested;

    private void Awake()
    {
        fields = GetComponentsInChildren<Field>();
        foreach (var field in fields)
        {
            var t = Random.Range(0, 5);
            field.Set((FieldType.GrowState)t);
        }
    }

    public int GrowCycle(Field.GrowChance growChance)
    {
        int corn = 0;
        foreach (var field in fields)
        {
            corn += field.Grow(growChance);
        }
        CornHarvested.Invoke(corn);
        return corn;
    }

    public bool IsActive => gameObject.activeSelf;

    public void Active(bool active)
    {
        gameObject.SetActive(active);
    }
}
