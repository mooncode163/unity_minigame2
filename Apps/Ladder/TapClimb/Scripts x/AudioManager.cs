using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceBackground;
    public AudioSource audioSource;

    public AudioClip backgroundMusic;
    public AudioClip catchStep, highlight, whoof, back, lose, click1,click2,comboLost;

    private void Start()
    {
        audioSourceBackground.clip = backgroundMusic;
        audioSourceBackground.loop = true;
        audioSourceBackground.playOnAwake = true;
        audioSourceBackground.Play();
        audioSourceBackground.volume = .4f;

        ///

        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void SetSoundState(bool active)
    {
        audioSource.mute = !active;
        audioSourceBackground.mute = !active;
    }

    void PlayAudioOneShot(AudioClip audioP)
    {
        audioSource.PlayOneShot(audioP);
    }

    public void PlayLose()
    {
        PlayAudioOneShot(lose);
    }
    public void PlayClick1()
    {
        PlayAudioOneShot(click1);
    }
    public void PlayClick2()
    {
        PlayAudioOneShot(click2);
    }

    public void PlayWhoof()
    {
        PlayAudioOneShot(whoof);
    }
    public void PlayBack()
    {
        PlayAudioOneShot(back);
    }
    public void PlayCatch()
    {
        PlayAudioOneShot(catchStep);
    }
    public void PlayHighlite()
    {
        PlayAudioOneShot(highlight);
    }
    public void PlayComboLost()
    {
        PlayAudioOneShot(comboLost);
    }

}
