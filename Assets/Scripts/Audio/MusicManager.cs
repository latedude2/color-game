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
        if (scene.name.Contains("Level") && scene.name != "LevelSelect") {
            if(scene.buildIndex < 6)
                SwitchSoundtrack(1);
            else if(scene.buildIndex >= 6 && scene.buildIndex < 12)
                SwitchSoundtrack(2);
            else if(scene.buildIndex >= 12 && scene.buildIndex < 23)
                SwitchSoundtrack(3);
            else if(scene.buildIndex >= 23)
                SwitchSoundtrack(3);
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
