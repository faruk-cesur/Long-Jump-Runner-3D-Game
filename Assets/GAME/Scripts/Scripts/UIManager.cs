using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public PlayerController player;

    public Slider distanceSlider, energySlider;

    public GameObject distanceFinish, particleCollectableGold, avoidObstaclesPanel, goldCoinPanel, energySliderObject,speedUpUI, speedDownUI,confettiParticle;

    public TextMeshProUGUI currentGoldText,
        earnedGoldText,
        earnedGoldBonusText,
        getGoldBonusXText,
        prepareTotalGoldText,
        winTotalGoldText,
        sliderLevelText;

    [HideInInspector] public int sliderLevel = 1, gold;

    [SerializeField] private GameObject _prepareGameUI,
        _mainGameUI,
        _loseGameUI,
        _winGameUI,
        _energySliderObject,
        _bonusPointArrow,
        _getGoldButton,
        _getBonusGoldButton,
        _bonusXArrow;

    [SerializeField] private List<GameObject> _goldenCoins;

    private bool _isBonusPointSelected;

    private float _anglerBonusArrowZ, _time = 1f;

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
                UpdateGoldInfo();
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
                CalculateRoadDistance();
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
    
    public void UpdateGoldInfo()
    {
        CalculateBonusArrowRotation();
        currentGoldText.text = gold.ToString();
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
        yield return new WaitForSeconds(player.longJumpTime + 2f);
        WinGameUI();
    }

    public IEnumerator DurationLoseGameUI()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitHeadSound, 1f);
        _energySliderObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.LoseGameSound();
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

    public void GetBonusGoldXButton()
    {
        foreach (var goldenCoin in _goldenCoins)
        {
            goldenCoin.SetActive(true);
            goldenCoin.transform.DOLocalMove(new Vector3(38.7f, -50f, 0), _time);
            _time += 0.05f;
        }
        
        if (_anglerBonusArrowZ <= 0 && _anglerBonusArrowZ >= -54f)
        {
            PlayerPrefs.SetInt("TotalGold", gold * 2 + PlayerPrefs.GetInt("TotalGold"));
        }

        if (_anglerBonusArrowZ < -54f && _anglerBonusArrowZ >= -110f)
        {
            PlayerPrefs.SetInt("TotalGold", gold * 3 + PlayerPrefs.GetInt("TotalGold"));
        }

        if (_anglerBonusArrowZ < -110f && _anglerBonusArrowZ >= -158f)
        {
            PlayerPrefs.SetInt("TotalGold", gold * 4 + PlayerPrefs.GetInt("TotalGold"));
        }

        if (_anglerBonusArrowZ < -158f && _anglerBonusArrowZ >= -180f)
        {
            PlayerPrefs.SetInt("TotalGold", gold * 5 + PlayerPrefs.GetInt("TotalGold"));
        }
        
        _getGoldButton.SetActive(false);
        _getBonusGoldButton.SetActive(false);
        _bonusXArrow.SetActive(false);
        NextLevelButton();
    }

    public void GetGoldButton()
    {
        foreach (var goldenCoin in _goldenCoins)
        {
            goldenCoin.SetActive(true);
            goldenCoin.transform.DOLocalMove(new Vector3(38.7f, -50f, 0), _time);
            _time += 0.05f;
        }

        PlayerPrefs.SetInt("TotalGold", gold + PlayerPrefs.GetInt("TotalGold"));
        _getGoldButton.SetActive(false);
        _getBonusGoldButton.SetActive(false);
        _bonusXArrow.SetActive(false);
        NextLevelButton();
    }

    private void CalculateBonusArrowRotation()
    {
        var anglerZ = UnityEditor.TransformUtils.GetInspectorRotation(_bonusPointArrow.transform).z;
        _anglerBonusArrowZ = anglerZ;
        if (anglerZ <= 0 && anglerZ >= -54f)
        {
            earnedGoldBonusText.text = (gold * 2).ToString();
            getGoldBonusXText.text = "GET X2";
        }

        if (anglerZ < -54f && anglerZ >= -110f)
        {
            earnedGoldBonusText.text = (gold * 3).ToString();
            getGoldBonusXText.text = "GET X3";
        }

        if (anglerZ < -110f && anglerZ >= -158f)
        {
            earnedGoldBonusText.text = (gold * 4).ToString();
            getGoldBonusXText.text = "GET X4";
        }

        if (anglerZ < -158f && anglerZ >= -180f)
        {
            earnedGoldBonusText.text = (gold * 5).ToString();
            getGoldBonusXText.text = "GET X5";
        }
    }
}