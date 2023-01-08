using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ActionButton : MonoBehaviour
{
    public enum WeatherEffect
    {
        Sun = 0,
        Night = 1,
        Rain = 2,
        Snow = 3,
        Lightning = 4,
        Whirlwind = 5,
        Cloudy = 6,
        CloudsOpenUp = 7,
        Meteor = 8,
    }

    public float Cooldown = 5f;
    public bool IsRunning = false;

    public GameObject overlay;
    public TextMeshProUGUI CDTime;

    public UnityEvent Click;

    private float resetCooldown = 5f;

    private void Awake()
    {
        overlay.SetActive(false);
        resetCooldown = Cooldown;
    }

    private void Update()
    {
        if (IsRunning)
        {
            if (!overlay.activeSelf)
            {
                overlay.SetActive(true);
            }
            if (Cooldown > 0)
            {
                Cooldown -= Time.deltaTime;
                CDTime.text = Mathf.CeilToInt(Cooldown).ToString();
            }
            else
            {
                Cooldown = resetCooldown;
                IsRunning = false;
                overlay.SetActive(false);
                CDTime.text = Mathf.CeilToInt(Cooldown).ToString();
            }
        }
    }

    public void Execute()
    {
        if (IsRunning)
            return;
        IsRunning = true;
        Click.Invoke();
    }
}
