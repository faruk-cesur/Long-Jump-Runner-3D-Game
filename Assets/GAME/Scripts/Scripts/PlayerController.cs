﻿using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Fields

    [HideInInspector] public bool finishCam;

    [HideInInspector] public float longJumpTime;

    [MinValue(7)] [MaxValue(12)] public float runSpeed;

    [SerializeField] private float _slideSpeed, _maxSlideAmount;

    [SerializeField] private Transform _playerModel;

    private float _jumpValue, _jumpDuration;

    private bool _isPlayerInteract, _isPlayerDead, _isPlayerWin, _isGameFinish;

    #endregion

    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                AnimationController.Instance.IdleAnimation();
                break;
            case GameState.MainGame:
                PlayerSpeedAnimations();
                ForwardMovement();
                SwerveMovement();
                PlayerDeathControl();
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
                ForwardMovement();
                StartCoroutine(PlayerWinBool());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region PlayerMovement

    private void ForwardMovement()
    {
        if (!_isPlayerWin)
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }
    }

    private float _mousePosX;
    private float _playerVisualPosX;

    private void SwerveMovement()
    {
        if (!_isPlayerInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerVisualPosX = _playerModel.localPosition.x;
                _mousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
            }

            if (Input.GetMouseButton(0))
            {
                float currentMousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
                float distance = currentMousePosX - _mousePosX;
                float posX = _playerVisualPosX + (distance * _slideSpeed);
                Vector3 pos = _playerModel.localPosition;
                pos.x = Mathf.Clamp(posX, -_maxSlideAmount, _maxSlideAmount);
                _playerModel.localPosition = pos;
            }
            else
            {
                Vector3 pos = _playerModel.localPosition;
            }
        }
    }

    #endregion

    #region PlayerTriggerEvents

    private void OnTriggerEnter(Collider other)
    {
        CollectableGold collectableGold = other.GetComponentInParent<CollectableGold>();
        if (collectableGold)
        {
            UIManager.Instance.gold++;
            Instantiate(UIManager.Instance.particleCollectableGold,
                _playerModel.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectableSound, 1f);
            Destroy(other.gameObject);
        }

        CollectableShoes collectableShoes = other.GetComponentInParent<CollectableShoes>();
        if (collectableShoes)
        {
            runSpeed += 0.25f;
            UIManager.Instance.energySlider.value += 0.25f;
            Instantiate(UIManager.Instance.particleCollectableGold,
                _playerModel.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectableSound, 1f);
            Destroy(other.gameObject);
        }

        Obstacle obstacle = other.GetComponentInParent<Obstacle>();
        if (obstacle)
        {
            runSpeed -= 0.25f;
            UIManager.Instance.energySlider.value -= 0.25f;
            Instantiate(UIManager.Instance.particleCollectableGold,
                _playerModel.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectableSound, 1f);
        }

        CollectShoesPanel collectShoesPanel = other.GetComponentInParent<CollectShoesPanel>();
        if (collectShoesPanel)
        {
            UIManager.Instance.goldCoinPanel.SetActive(false);
            UIManager.Instance.avoidObstaclesPanel.SetActive(true);
        }

        AvoidObstaclePanel avoidObstaclePanel = other.GetComponentInParent<AvoidObstaclePanel>();
        if (avoidObstaclePanel)
        {
            UIManager.Instance.avoidObstaclesPanel.SetActive(false);
            UIManager.Instance.pickObjectPanel.SetActive(true);
        }

        PickObjectPanel pickObjectPanel = other.GetComponentInParent<PickObjectPanel>();
        if (pickObjectPanel)
        {
            UIManager.Instance.pickObjectPanel.SetActive(false);
        }
    }

    #endregion


    #region Methods

    public void PlayerSpeedDown()
    {
        runSpeed = 10;
    }

    private void PlayerDeathControl()
    {
        if (UIManager.Instance.energySlider.value <= 7 && !_isPlayerDead)
        {
            _isPlayerDead = true;
            GameManager.Instance.LoseGame();
        }
    }

    private void PlayerSpeedAnimations()
    {
        if (runSpeed < 8.25f)
        {
            AnimationController.Instance.WalkAnimation();
        }
        else if (runSpeed >= 8.25f && runSpeed < 10)
        {
            AnimationController.Instance.SlowRunAnimation();
        }
        else if (runSpeed >= 10)
        {
            AnimationController.Instance.RunAnimation();
        }
    }

    public void LongJumpCalculate()
    {
        if (runSpeed < 8.25f)
        {
            longJumpTime = 1f;
            _jumpValue = 2f;
            _jumpDuration = 0.5f;
        }
        else if (runSpeed >= 8.25f && runSpeed < 9)
        {
            longJumpTime = 2f;
            _jumpValue = 4f;
            _jumpDuration = 1f;
        }
        else if (runSpeed >= 9 && runSpeed < 10)
        {
            longJumpTime = 3f;
            _jumpValue = 6f;
            _jumpDuration = 1.5f;
        }
        else if (runSpeed >= 10 && runSpeed < 10.75f)
        {
            longJumpTime = 4f;
            _jumpValue = 8f;
            _jumpDuration = 2f;
        }
        else if (runSpeed >= 10.75f && runSpeed < 11.75f)
        {
            longJumpTime = 5f;
            _jumpValue = 10f;
            _jumpDuration = 2.5f;
        }
        else if (runSpeed >= 11.75f)
        {
            longJumpTime = 6f;
            _jumpValue = 10f;
            _jumpDuration = 3f;
        }
    }

    #endregion


    #region Coroutines

    private IEnumerator PlayerInteractBool()
    {
        _isPlayerInteract = true;
        yield return new WaitForSeconds(1f);
        _isPlayerInteract = false;
    }

    private IEnumerator PlayerWinBool()
    {
        if (!_isGameFinish)
        {
            _isGameFinish = true;
            _playerModel.DOMoveX(0, 1);
            StartCoroutine(JumpingPositionY());
            StartCoroutine(AnimationController.Instance.JumpAnimation());
            yield return new WaitForSeconds(longJumpTime);
            AnimationController.Instance.LandAnimation();
            _isPlayerWin = true;
            yield return new WaitForSeconds(1f);
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, -130, 0);
            _playerModel.localRotation = rot;
            AnimationController.Instance.WinAnimation();
            SoundManager.Instance.WinGameSound();
        }
    }

    private IEnumerator JumpingPositionY()
    {
        _playerModel.DOMoveY(_jumpValue, _jumpDuration);
        yield return new WaitForSeconds(_jumpDuration);
        _playerModel.DOMoveY(0, _jumpDuration);
    }

    #endregion
}