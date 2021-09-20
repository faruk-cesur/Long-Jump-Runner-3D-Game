using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip collectGoldSound, loseGameSound, winGameSound, hitHeadSound, collectShoesSound, windWalkSound, beforeJumpSound;

    private AudioSource _audioSource;


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

        _audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
    
    public void WinGameSound()
    {
        _audioSource.clip = winGameSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void WindWalkSoundPlay()
    {
        _audioSource.clip = windWalkSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    public void WindWalkSoundStop()
    {
        if (_audioSource.clip == windWalkSound)
        {
            _audioSource.clip = null;
        }
    }
}