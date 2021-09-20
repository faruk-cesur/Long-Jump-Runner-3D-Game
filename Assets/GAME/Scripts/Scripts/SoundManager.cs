using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip collectGoldSound, loseGameSound, winGameSound, hitHeadSound, collectShoesSound, windWalkSound, beforeJumpSound;

    [HideInInspector] public AudioSource audioSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
    
    public void WinGameSound()
    {
        audioSource.clip = winGameSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void WindWalkSoundPlay()
    {
        audioSource.clip = windWalkSound;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void WindWalkSoundStop()
    {
        if (audioSource.clip == windWalkSound)
        {
            audioSource.clip = null;
        }
    }
}