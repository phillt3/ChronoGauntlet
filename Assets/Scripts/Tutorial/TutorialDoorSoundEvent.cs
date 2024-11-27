using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorSoundEvent : MonoBehaviour
{
    public AudioClip sound;
    public AudioSource audioSource;

    public void PlaySound()
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}