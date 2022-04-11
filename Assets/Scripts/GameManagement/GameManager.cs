using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public static bool gamePaused = false;
    public DateTime session;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start() {
        Application.targetFrameRate = 60;
        session = DateTime.Now;
    }

    public static void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }
}
