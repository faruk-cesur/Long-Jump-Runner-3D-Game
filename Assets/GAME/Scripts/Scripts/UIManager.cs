using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public PlayerController player;

    public Slider distanceSlider, energySlider;

    public GameObject distanceFinish, particleCollectableGold, pickObjectPanel, avoidObstaclesPanel, goldCoinPanel;

    public TextMeshProUGUI currentGoldText, earnedGoldText, prepareTotalGoldText,winTotalGoldText, sliderLevelText;

    [HideInInspector] public int sliderLevel = 1, gold;

    [SerializeField] private GameObject _prepareGameUI, _mainGameUI, _loseGameUI, _winGameUI, _energySliderObject;

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
        SetGoldZeroOnStart();
        SetPlayerPrefs();
    }

    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                PrepareGameUI();
                UpdateGoldInfo();
                break;
            case GameState.MainGame:
                CalculateRoadDistance();
                EqualCurrentGold();
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
                UpdateGoldInfo();
                break;
        }
    }

    public void PrepareGameUI()
    {
        _energySliderObject.SetActive(false);
        _prepareGameUI.SetActive(true);
        _mainGameUI.SetActive(false);
        _loseGameUI.SetActive(false);
        _winGameUI.SetActive(false);
    }

    public void MainGameUI()
    {
        _energySliderObject.SetActive(true);
        _prepareGameUI.SetActive(false);
        _mainGameUI.SetActive(true);
        _loseGameUI.SetActive(false);
        _winGameUI.SetActive(false);
    }

    public void LoseGameUI()
    {
        _energySliderObject.SetActive(false);
        _prepareGameUI.SetActive(false);
        _mainGameUI.SetActive(false);
        _loseGameUI.SetActive(true);
        _winGameUI.SetActive(false);
    }

    public void WinGameUI()
    {
        _energySliderObject.SetActive(false);
        _prepareGameUI.SetActive(false);
        _mainGameUI.SetActive(false);
        _loseGameUI.SetActive(false);
        _winGameUI.SetActive(true);
    }

    private void CalculateRoadDistance()
    {
        distanceSlider.maxValue = distanceFinish.gameObject.transform.localPosition.z;
        distanceSlider.value = player.gameObject.transform.localPosition.z;
    }

    private void SetGoldZeroOnStart()
    {
        gold = 0;
    }

    private void EqualCurrentGold()
    {
        currentGoldText.text = gold.ToString();
    }

    public void UpdateGoldInfo()
    {
        earnedGoldText.text = currentGoldText.text;
        prepareTotalGoldText.text = PlayerPrefs.GetInt("TotalGold").ToString();
        winTotalGoldText.text = PlayerPrefs.GetInt("TotalGold").ToString();
    }

    private void SetPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("TotalGold"))
        {
            PlayerPrefs.SetInt("TotalGold", gold);
        }

        if (!PlayerPrefs.HasKey("SliderLevel"))
        {
            PlayerPrefs.SetInt("SliderLevel", sliderLevel);
        }

        sliderLevelText.text = PlayerPrefs.GetInt("SliderLevel").ToString();
    }

    public IEnumerator DurationWinGameUI()
    {
        yield return new WaitForSeconds(2f);
        WinGameUI();
    }

    public IEnumerator DurationLoseGameUI()
    {
        yield return new WaitForSeconds(2f);
        LoseGameUI();
    }

    public void RetryButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("SliderLevel", PlayerPrefs.GetInt("SliderLevel") + 1);
        sliderLevelText.text = PlayerPrefs.GetInt("SliderLevel").ToString();
        LevelManager.Instance.NextLevel();
    }
}