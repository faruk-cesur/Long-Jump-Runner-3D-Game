using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParent : MonoBehaviour
{
    private void Start()
    {
        WithForeachLoop();
    }

    private void WithForeachLoop()
    {
        foreach (Transform child in transform)
        {
            SettingsManager.Instance.obstacleListForImmortality.Add(child.gameObject);
            SettingsManager.Instance.obstacleListForSpeed.Add(child.gameObject.GetComponent<Animator>());
        }
    }
}
