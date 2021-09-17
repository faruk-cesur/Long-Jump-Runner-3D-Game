using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private bool _isJump;
    private void Update()
    {
        WinMiniGame();
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponentInParent<PlayerController>();
        if (playerController)
        {
            playerController.finishCam = true;
            playerController.PlayerSpeedDown();
            PlayerPrefs.SetInt("TotalGold", UIManager.Instance.gold + PlayerPrefs.GetInt("TotalGold"));
            AnimationController.Instance.WinAnimation();
            UIManager.Instance.UpdateGoldInfo();
            GameManager.Instance.WinGame();
            GameManager.Instance.CurrentGameState = GameState.WinGame;
            SoundManager.Instance.PlaySound(SoundManager.Instance.winGameSound, 1);
        }
    }

    private void WinMiniGame()
    {
        if (!_isJump && _player.transform.position.z < transform.position.z + 15.01f && _player.transform.position.z > transform.position.z + 4.99f)
        {
            Debug.Log("X1");
            _isJump = true;
        }
        else if (!_isJump && _player.transform.position.z < transform.position.z + 25.01f && _player.transform.position.z > transform.position.z + 14.99f)
        {
            Debug.Log("X2");
            _isJump = true;
        }
        else if (!_isJump && _player.transform.position.z < transform.position.z + 35.01f && _player.transform.position.z > transform.position.z + 24.99f)
        {
            Debug.Log("X4");
            _isJump = true;
        }
        else if (!_isJump && _player.transform.position.z < transform.position.z + 45.01f && _player.transform.position.z > transform.position.z + 34.99f)
        {
            Debug.Log("X6");
            _isJump = true;
        }
        else if (!_isJump && _player.transform.position.z < transform.position.z + 55.01f && _player.transform.position.z > transform.position.z + 44.99f)
        {
            Debug.Log("X8");
            _isJump = true;
        }
        else if (!_isJump && _player.transform.position.z < transform.position.z + 65.01f && _player.transform.position.z > transform.position.z + 54.99f)
        {
            Debug.Log("X10");
            _isJump = true;
        }
    }
}