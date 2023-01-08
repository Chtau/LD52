using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceBackground;
    public AudioSource audioSource;
    public AudioClip AudioClip;
    public AudioClip blipSound;

    private float sfxSound = 0.5f;

    private void Awake()
    {
        var b = PlayerPrefs.GetFloat(Options.KEY_BACKGROUND_SOUND, .5f);
        audioSourceBackground.volume = b / 10;
        var sfx = PlayerPrefs.GetFloat(Options.KEY_SOUND_EFFECT, .5f);
        sfxSound = sfx;

        //audioSourceBackground.volume = 0.05f;
        audioSourceBackground.loop = true;
        audioSourceBackground.clip = AudioClip;
        audioSourceBackground.Play();
    }

    public void BackgroundChanged(float value)
    {
        audioSourceBackground.volume = value / 10;
    }

    public void SFXChanged(float value)
    {
        sfxSound = value;
    }

    public void PlayBlipSound()
    {
        audioSource.PlayOneShot(blipSound, sfxSound);
    }
}
