using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

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
        _instance.StartCoroutine(PauseGameAudio());
    }

    public static void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        _instance.StartCoroutine(ResumeGameAudio());
    }

    public static IEnumerator PauseGameAudio()
    {
        // We delay the pause to avoid clicks. setVolume does not produce clicks where as setPaused does
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay");
        bus.setVolume(0.01f); // Setting to small number instead of zero to avoid clicks when resuming
        yield return new WaitForEndOfFrame();
        bus.setPaused(true);
    }

    public static IEnumerator ResumeGameAudio()
    {
        // We delay the resume to avoid clicks. setVolume does not produce clicks where as setPaused does
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay");
        bus.setPaused(false);
        yield return new WaitForEndOfFrame();
        bus.setVolume(1);
    }
}
