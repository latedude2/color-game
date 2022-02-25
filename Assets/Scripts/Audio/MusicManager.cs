using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MusicManager : MonoBehaviour {

    FMODUnity.StudioEventEmitter soundtrack;
    public int currMusic = 0;
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake() {
        soundtrack = GetComponent<StudioEventEmitter>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoaded");
        if (scene.name.Contains("Level") && scene.name != "LevelSelect") {
            Debug.Log("Choosing soundtrack");
            if(scene.buildIndex < 8)
                SwitchSoundtrack(1);
            else if(scene.buildIndex >= 8 && scene.buildIndex < 14)
                SwitchSoundtrack(2);
            else if(scene.buildIndex >= 14)
                SwitchSoundtrack(3);
            Debug.Log(" soundtrack chosen");
            
        }
    }

    public void NextSoundtrack() {
        SwitchSoundtrack(currMusic+1);
    }

    public void SwitchSoundtrack(int to) {
        soundtrack.SetParameter("Index", to);
        currMusic = to;
    }
}
