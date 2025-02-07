using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioSource : MonoBehaviour
{
    public AudioSource PlayButtonAudioSource,QuitButtonAudioSource;

    void Start()
    {
    }

    public void PlayButtonSound()
    {
        PlayButtonAudioSource.Play();
    }

    public void QuitButtonSound()
    {
        QuitButtonAudioSource.Play();
    }
}

