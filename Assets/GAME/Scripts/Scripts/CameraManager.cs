using System;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static Camera Cam;
    public static CameraManager Instance;

    public GameObject prepareGameCam, mainGameCam,longJumpCam, winGameCam, loseGameCam;

    private void Awake()
    {
        Cam = Camera.main;
        Instance = this;
    }

    private void Start()
    {
        PrepareGameCamera();
    }
    
    public void PrepareGameCamera()
    {
        prepareGameCam.SetActive(true);
        mainGameCam.SetActive(false);
        longJumpCam.SetActive(false);
        winGameCam.SetActive(false);
        loseGameCam.SetActive(false);
    }
    public void MainGameCamera()
    {
        prepareGameCam.SetActive(false);
        mainGameCam.SetActive(true);
        longJumpCam.SetActive(false);
        winGameCam.SetActive(false);
        loseGameCam.SetActive(false);
    }

    public void LongJumpCamera()
    {
        prepareGameCam.SetActive(false);
        mainGameCam.SetActive(false);
        longJumpCam.SetActive(true);
        winGameCam.SetActive(false);
        loseGameCam.SetActive(false);
    }
    
    public void WinGameCamera()
    {
        prepareGameCam.SetActive(false);
        mainGameCam.SetActive(false);
        longJumpCam.SetActive(false);
        winGameCam.SetActive(true);
        loseGameCam.SetActive(false);
    }
    
    public void LoseGameCamera()
    {
        prepareGameCam.SetActive(false);
        mainGameCam.SetActive(false);
        longJumpCam.SetActive(false);
        winGameCam.SetActive(false);
        loseGameCam.SetActive(true);
    }
}