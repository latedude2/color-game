using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private static LevelManager _instance;
     public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("LevelManager is missing!");
            return _instance;
        }
    }

    private void Awake() {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        if (ColorGame.Debug.debugMode && Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SaveProgress(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
        else 
        {
            
            SceneManager.LoadScene(0);
            Settings.UnlockCursor();
        }
        
    }

    private void SaveProgress(int levelIndex)
    {
        PlayerPrefs.SetInt("LastCompletedLevel", levelIndex);
        PlayerPrefs.Save();
    }

    public void LoadProgressLevel()
    {
        int levelIndex = PlayerPrefs.GetInt("LastCompletedLevel", 1);       //load first level if there is no progress saved
        Debug.Log("Loading level:" + levelIndex);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
