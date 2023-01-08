using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static string KEY_BACKGROUND_SOUND = "backgroundsound";
    public static string KEY_SOUND_EFFECT = "sfx";

    public Slider backgroundSound;
    public Slider soundEffect;

    public UnityEvent<float> BackgroundSoundChangedTriggerd;
    public UnityEvent<float> SoundEffectTriggerd;

    private void Awake()
    {
        var b = PlayerPrefs.GetFloat(KEY_BACKGROUND_SOUND, .5f);
        backgroundSound.value = b;
        BackgroundSoundChangedTriggerd.Invoke(b);
        var sfx = PlayerPrefs.GetFloat(KEY_SOUND_EFFECT, .5f);
        soundEffect.value = sfx;
        SoundEffectTriggerd.Invoke(sfx);
    }

    public void BackgroundSoundChanged(Single value)
    {
        PlayerPrefs.SetFloat(KEY_BACKGROUND_SOUND, value);
        PlayerPrefs.Save();
        BackgroundSoundChangedTriggerd.Invoke(value);
    }

    public void SoundEffectChanged(Single value)
    {
        PlayerPrefs.SetFloat(KEY_SOUND_EFFECT, value);
        PlayerPrefs.Save();
        SoundEffectTriggerd.Invoke(value);
    }
}
