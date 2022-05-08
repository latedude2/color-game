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

    void Start() {
        SwitchSoundtrack(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name.Contains("Level") && scene.name != "LevelSelect")
            SwitchSoundtrack(scene.buildIndex);
    }

    public void NextSoundtrack() {
        SwitchSoundtrack(currMusic+1);
    }

    public void SwitchSoundtrack(int to) {
        if(to < 6)
            soundtrack.SetParameter("Index", 1);
        else if(to >= 6 && to < 12)
            soundtrack.SetParameter("Index", 2);
        else if(to >= 12 && to < 23)
            soundtrack.SetParameter("Index", 3);
        else if(to >= 23)
            soundtrack.SetParameter("Index", 4);
        currMusic = to;
    }
}
