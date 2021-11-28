using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public AudioSource[] music;
    public int currMusic = 0;
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name.Contains("Level") && scene.name != "LevelSelect") {
            if(scene.buildIndex < 8)
                SwitchSoundtrack(0);
            else if(scene.buildIndex >= 8 && scene.buildIndex < 14)
                SwitchSoundtrack(1);
            else if(scene.buildIndex >= 14)
                SwitchSoundtrack(2);
        }
    }

    void Update() {
        // if (Input.GetKeyDown(KeyCode.M)) {
        //     SwitchSoundtrack(currMusic+1);
        //     // ss2.TransitionTo(5);
        // }
        // if (Input.GetKeyDown(KeyCode.N)) {
        //     // ss1.TransitionTo(5);
        // }
    }

    public void NextSoundtrack() {
        SwitchSoundtrack(currMusic+1);
    }

    public void SwitchSoundtrack(int to) {
        if (to == currMusic)
            return;
        if (music == null)
            return;
        if (to >= music.Length || to < 0)
            return;
        StartCoroutine(Crossfade(currMusic, to));
        currMusic = to;
    }

    IEnumerator Crossfade(int from, int to) {
        bool crossfading = true;
        music[to].Play();
        while (crossfading) {
            music[from].volume -= 0.02f;
            music[to].volume += 0.02f;
            if(music[to].volume == 1) {
                music[from].volume = 0;
                music[from].Stop();
                crossfading = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
