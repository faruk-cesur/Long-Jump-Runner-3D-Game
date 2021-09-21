using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Fields

    [HideInInspector] public float longJumpTime, shoesSpeedUp = 0.25f, obstacleDamage = 1f;

    [MinValue(7)] [MaxValue(12)] public float runSpeed;

    [SerializeField] private float _slideSpeed, _maxSlideAmount;

    [SerializeField] private Transform _playerVisual, _playerModel;

    private float _jumpValue, _jumpDuration;

    private bool _isPlayerInteract, _isPlayerDead, _isPlayerWin, _isGameFinish, _isWindWalkActive, _isGameStarted;

    #endregion
    

    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                AnimationController.Instance.IdleAnimation();
                break;
            case GameState.MainGame:
                PlayerResetPositionOnStart();
                PlayerSpeedCalculate();
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
                _playerVisualPosX = _playerVisual.localPosition.x;
                _mousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
            }

            if (Input.GetMouseButton(0))
            {
                float currentMousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
                float distance = currentMousePosX - _mousePosX;
                float posX = _playerVisualPosX + (distance * _slideSpeed);
                Vector3 pos = _playerVisual.localPosition;
                pos.x = Mathf.Clamp(posX, -_maxSlideAmount, _maxSlideAmount);
                _playerVisual.localPosition = pos;
            }
            else
            {
                Vector3 pos = _playerVisual.localPosition;
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
                _playerVisual.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectGoldSound, 1f);
            Destroy(other.gameObject);
        }

        CollectableShoes collectableShoes = other.GetComponentInParent<CollectableShoes>();
        if (collectableShoes)
        {
            runSpeed += shoesSpeedUp;
            UIManager.Instance.energySlider.value += shoesSpeedUp;
            StartCoroutine(SpeedUpParticle());
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectShoesSound, 1f);
            Destroy(other.gameObject);
            //Taptic.Medium();
        }

        Obstacle obstacle = other.GetComponentInParent<Obstacle>();
        if (!_isPlayerInteract)
        {
            if (obstacle)
            {
                StartCoroutine(PlayerInteractBool());
                runSpeed -= obstacleDamage;
                UIManager.Instance.energySlider.value -= obstacleDamage;
                StartCoroutine(SpeedDownParticle());
                StartCoroutine(AnimationController.Instance.InjuredRun());
                SoundManager.Instance.PlaySound(SoundManager.Instance.hitHeadSound, 1f);
                //Taptic.Heavy();
            }
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
        }
    }

    #endregion


    #region Methods

    public void PlayerSpeedDown()
    {
        runSpeed = 10f;
        UIManager.Instance.energySlider.value = 10f;
    }

    private void PlayerDeathControl()
    {
        if (UIManager.Instance.energySlider.value <= 7 && !_isPlayerDead)
        {
            _isPlayerDead = true;
            GetComponentInChildren<Collider>().enabled = false;
            GameManager.Instance.LoseGame();
        }
    }

    private void PlayerSpeedCalculate()
    {
        if (runSpeed < 8.25f)
        {
            AnimationController.Instance.WalkAnimation();
            _isWindWalkActive = false;
            SoundManager.Instance.WindWalkSoundStop();
        }
        else if (runSpeed >= 8.25f && runSpeed < 10)
        {
            AnimationController.Instance.SlowRunAnimation();
            _isWindWalkActive = false;
            SoundManager.Instance.WindWalkSoundStop();
        }
        else if (runSpeed >= 10)
        {
            AnimationController.Instance.RunAnimation();
            if (!_isWindWalkActive)
            {
                _isWindWalkActive = true;
                SoundManager.Instance.WindWalkSoundPlay();
            }
        }
    }

    public void LongJumpCalculate()
    {
        if (runSpeed < 8.25f)
        {
            longJumpTime = 1f;
            _jumpValue = 2f;
            _jumpDuration = 0.5f;
            UIManager.Instance.getGoldText.text = "GET X1";
        }
        else if (runSpeed >= 8.25f && runSpeed < 9)
        {
            longJumpTime = 2f;
            _jumpValue = 4f;
            _jumpDuration = 1f;
            UIManager.Instance.getGoldText.text = "GET X2";
        }
        else if (runSpeed >= 9 && runSpeed < 10)
        {
            longJumpTime = 3f;
            _jumpValue = 6f;
            _jumpDuration = 1.5f;
            UIManager.Instance.getGoldText.text = "GET X4";
        }
        else if (runSpeed >= 10 && runSpeed < 10.75f)
        {
            longJumpTime = 4f;
            _jumpValue = 8f;
            _jumpDuration = 2f;
            UIManager.Instance.getGoldText.text = "GET X6";
        }
        else if (runSpeed >= 10.75f && runSpeed < 11.75f)
        {
            longJumpTime = 5f;
            _jumpValue = 10f;
            _jumpDuration = 2.5f;
            UIManager.Instance.getGoldText.text = "GET X8";
        }
        else if (runSpeed >= 11.75f)
        {
            longJumpTime = 6f;
            _jumpValue = 10f;
            _jumpDuration = 3f;
            UIManager.Instance.getGoldText.text = "GET X10";
        }
    }

    private void PlayerResetPositionOnStart()
    {
        if (!_isGameStarted)
        {
            _isGameStarted = true;
            _playerModel.transform.position = new Vector3(0, 0, 3);
            _playerModel.transform.rotation = Quaternion.identity;
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
            //Taptic.Warning();
            CameraManager.Instance.LongJumpCamera();
            SoundManager.Instance.WindWalkSoundStop();
            SoundManager.Instance.PlaySound(SoundManager.Instance.beforeJumpSound,1f);
            _playerVisual.DOMoveX(0, 1);
            StartCoroutine(JumpingPositionY());
            StartCoroutine(AnimationController.Instance.JumpAnimation());
            yield return new WaitForSeconds(longJumpTime);
            AnimationController.Instance.LandAnimation();
            _isPlayerWin = true;
            yield return new WaitForSeconds(1f);
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, -130, 0);
            _playerVisual.localRotation = rot;
            AnimationController.Instance.WinAnimation();
            SoundManager.Instance.WinGameSound();
            Instantiate(UIManager.Instance.confettiParticle, _playerVisual.transform.position + new Vector3(0,5,0), Quaternion.identity);
            CameraManager.Instance.WinGameCamera();
            //Taptic.Success();
        }
    }

    private IEnumerator JumpingPositionY()
    {
        _playerVisual.DOMoveY(_jumpValue, _jumpDuration);
        yield return new WaitForSeconds(_jumpDuration);
        _playerVisual.DOMoveY(0, _jumpDuration);
    }
    private IEnumerator SpeedUpParticle()
    {
        UIManager.Instance.speedUpUI.SetActive(true);
        UIManager.Instance.speedUpUI.transform.DOMoveY(6, 0.5f);
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.speedUpUI.transform.DOMoveY(3, 0.01f);
        UIManager.Instance.speedUpUI.SetActive(false);
    }
    
    private IEnumerator SpeedDownParticle()
    {
        UIManager.Instance.speedDownUI.SetActive(true);
        UIManager.Instance.speedDownUI.transform.DOMoveY(6, 0.5f);
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.speedDownUI.transform.DOMoveY(3, 0.01f);
        UIManager.Instance.speedDownUI.SetActive(false);
    }

    #endregion
}