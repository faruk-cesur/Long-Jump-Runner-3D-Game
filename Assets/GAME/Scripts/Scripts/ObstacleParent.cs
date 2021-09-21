using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParent : MonoBehaviour
{
    public PlayerController player;
    private float playerPosZ;
    private void Start()
    {
        WithForeachLoop();
    }

    private void Update()
    {
        CalculatePlayerPosition();
        DisappearObstacle();
    }

    private void WithForeachLoop()
    {
        foreach (Transform child in transform)
        {
            SettingsManager.Instance.obstacleListForImmortality.Add(child.gameObject);
            SettingsManager.Instance.obstacleListForSpeed.Add(child.gameObject.GetComponent<Animator>());
        }
    }

    private void CalculatePlayerPosition()
    {
        playerPosZ = player.transform.position.z;
    }

    private void DisappearObstacle()
    {
        foreach (var obstacle in SettingsManager.Instance.obstacleListForImmortality)
        {
            if (obstacle.transform.position.z < playerPosZ -10)
            {
                obstacle.SetActive(false);
            }
        }
    }
}
