using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public PlayerController playerController;

    public Slider sliderVibration;
    public Slider sliderAudio;
    public Slider sliderStartingSpeed;
    public Slider sliderShoesSpeedUp;
    public Slider sliderObstacleSpeed;
    public Slider sliderObstacleDamage;
    public Slider sliderImmortality;

    public GameObject settingsPanel;

    public List<Animator> obstacleListForSpeed;
    public List<GameObject> obstacleListForImmortality;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        FirstStart();
        Audio();
        sliderVibration.value = PlayerPrefs.GetFloat("Vibration");
        sliderAudio.value = PlayerPrefs.GetFloat("Audio");
        StartingPlayerPrefs();
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        UpdatePlayerPrefs();
        settingsPanel.SetActive(false);
        GameManager.Instance.RestartGame();
    }

    public void DefaultSettings()
    {
        sliderVibration.value = 1f;
        sliderAudio.value = 1f;
        sliderStartingSpeed.value = 7.75f;
        sliderShoesSpeedUp.value = 0.25f;
        sliderObstacleSpeed.value = 1f;
        sliderObstacleDamage.value = 1f;
        sliderImmortality.value = 0f;
    }

    private void FirstStart()
    {
        if (!PlayerPrefs.HasKey("StartingSpeed"))
        {
            DefaultSettings();
            UpdatePlayerPrefs();
        }
    }

    private void UpdatePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Vibration", sliderVibration.value);
        PlayerPrefs.SetFloat("Audio", sliderAudio.value);
        PlayerPrefs.SetFloat("StartingSpeed", sliderStartingSpeed.value);
        PlayerPrefs.SetFloat("ShoesSpeedUp", sliderShoesSpeedUp.value);
        PlayerPrefs.SetFloat("ObstacleSpeed", sliderObstacleSpeed.value);
        PlayerPrefs.SetFloat("ObstacleDamage", sliderObstacleDamage.value);
        PlayerPrefs.SetFloat("Immortality", sliderImmortality.value);
    }

    private void StartingPlayerPrefs()
    {
        StartingSpeed();
        ShoesSpeedUp();
        ObstacleSpeed();
        ObstacleDamage();
        Immortality();
    }

    public void Vibration()
    {
        if (sliderVibration.value == 0)
        {
            Taptic.tapticOn = false;
        }
        else
        {
            Taptic.tapticOn = true;
        }
    }

    public void Audio()
    {
        if (sliderAudio.value == 0)
        {
            SoundManager.Instance.audioSource.mute = true;
        }
        else
        {
            SoundManager.Instance.audioSource.mute = false;
        }
    }

    private void StartingSpeed()
    {
        sliderStartingSpeed.value = PlayerPrefs.GetFloat("StartingSpeed");
        playerController.runSpeed = PlayerPrefs.GetFloat("StartingSpeed");
        UIManager.Instance.energySlider.value = PlayerPrefs.GetFloat("StartingSpeed");
    }

    private void ShoesSpeedUp()
    {
        sliderShoesSpeedUp.value = PlayerPrefs.GetFloat("ShoesSpeedUp");
        playerController.shoesSpeedUp = PlayerPrefs.GetFloat("ShoesSpeedUp");
    }

    private void ObstacleSpeed()
    {
        sliderObstacleSpeed.value = PlayerPrefs.GetFloat("ObstacleSpeed");

        foreach (var obstacle in obstacleListForSpeed)
        {
            obstacle.speed = PlayerPrefs.GetFloat("ObstacleSpeed");
        }
    }

    private void ObstacleDamage()
    {
        sliderObstacleDamage.value = PlayerPrefs.GetFloat("ObstacleDamage");
        playerController.obstacleDamage = PlayerPrefs.GetFloat("ObstacleDamage");
    }

    private void Immortality()
    {
        sliderImmortality.value = PlayerPrefs.GetFloat("Immortality");

        if (PlayerPrefs.GetFloat("Immortality") == 0)
        {
            foreach (var obstacle in obstacleListForImmortality)
            {
                obstacle.GetComponentInChildren<Collider>().enabled = true;
            }
        }
        else
        {
            foreach (var obstacle in obstacleListForImmortality)
            {
                obstacle.GetComponentInChildren<Collider>().enabled = false;
            }
        }
    }
}