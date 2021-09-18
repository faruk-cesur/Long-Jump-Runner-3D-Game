using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Fields

    [HideInInspector] public bool finishCam;

    [MinValue(7)] [MaxValue(12)] public float runSpeed;

    [SerializeField] private float _slideSpeed, _maxSlideAmount;

    [SerializeField] private Transform _playerModel;

    private bool _isPlayerInteract, _isPlayerDead, _isPlayerWin;

    [SerializeField] private float _longJumpTime;

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

    private void PlayerDeath()
    {
        runSpeed = 0;
        GameManager.Instance.LoseGame();
        AnimationController.Instance.DeathAnimation();
        StartCoroutine(SoundManager.Instance.LoseGameSound());
        GameManager.Instance.CurrentGameState = GameState.LoseGame;
        _isPlayerDead = true;
        _isPlayerInteract = true;
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
            _longJumpTime = 1f;
        }
        else if (runSpeed >= 8.25f && runSpeed < 9)
        {
            _longJumpTime = 2f;
        }
        else if (runSpeed >= 9 && runSpeed < 10)
        {
            _longJumpTime = 3f;
        }
        else if (runSpeed >= 10 && runSpeed < 10.75f)
        {
            _longJumpTime = 4f;
        }
        else if (runSpeed >= 10.75f && runSpeed < 11.75f)
        {
            _longJumpTime = 5f;
        }
        else if (runSpeed >= 11.75f)
        {
            _longJumpTime = 6f;
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
        yield return new WaitForSeconds(_longJumpTime);
        _isPlayerWin = true;
    }

    #endregion
}