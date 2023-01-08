using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EventDisplay : MonoBehaviour
{
    public TextMeshProUGUI msgText;
    public GameObject msgImage;

    public TextMeshProUGUI msgDialogText;
    public GameObject msgDialogImage;

    public TextMeshProUGUI farmerEventText;
    public GameObject farmerEventCanvas;

    public UnityEvent ShouldPlaySound;

    private void Awake()
    {
        msgImage.SetActive(false);
        msgDialogImage.SetActive(false);
        farmerEventCanvas.SetActive(false);
    }

    public void Message(string msg)
    {
        msgText.text = msg;
        msgImage.SetActive(true);
        PlayInfoSound();
        msgImage.LeanDelayedCall(2f, () =>
        {
            msgImage.SetActive(false);
        });
    }

    public void MessageDialog(string msg)
    {
        msgDialogText.text = msg;
        msgDialogImage.SetActive(true);
    }

    public void CloseDialog()
    {
        msgDialogImage.SetActive(false);
    }

    public void FarmerMessage(string msg)
    {
        farmerEventText.text = msg;
        farmerEventCanvas.SetActive(true);
        PlayInfoSound();
        farmerEventCanvas.LeanDelayedCall(2f, () =>
        {
            farmerEventCanvas.SetActive(false);
        });
    }

    private void PlayInfoSound()
    {
        ShouldPlaySound.Invoke();
    }
}
