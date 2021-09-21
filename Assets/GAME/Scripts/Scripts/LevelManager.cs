using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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
    }

    public List<GameObject> levels;

    [HideInInspector] public int currentLevel;

    private void Start()
    {
        SetLevelPlayerPrefs();
        CallLevel();
    }

    private void Update()
    {
        //PlayerPrefs.DeleteAll();
    }

    public void SetLevelPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        }
    }


    public void CallLevel()
    {
        for (int i = 0; i < levels.Count + 1; i++)
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == i)
            {
                levels[i - 1].SetActive(true);

                if (PlayerPrefs.GetInt("CurrentLevel") != 1)
                {
                    levels[i - 2].SetActive(false);
                }
            }
        }
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}