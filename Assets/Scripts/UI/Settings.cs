using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public delegate void PointerHandler();
    public static event PointerHandler Locked;
    public static event PointerHandler Unlocked;
    private AudioSettings audioSettings;
    private VideoSettings videoSettings;

    private ControlSettings controlSettings;

    private bool settingsOpen = false;
    private GameObject optionsMenu;
    private GameObject targetReticle;

    private void Start() {
        optionsMenu = transform.Find("Options").gameObject;
        targetReticle = transform.Find("TargetReticle").gameObject;
        Locked += LockCursor;
        Unlocked += UnlockCursor;
        audioSettings = GetComponent<AudioSettings>();
        controlSettings = GetComponent<ControlSettings>();
        videoSettings = GetComponent<VideoSettings>();
        LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingsOpen)
            {
                HideSettings();
            }
            else
            {
                ShowSettings();
            }
        }
    }

    void OnDestroy() {
        //It is a good practice to unsubscribe events when the objects are destroyed to avoid errors.
        Locked -= LockCursor;
        Unlocked -= UnlockCursor;
    }

    void LoadSettings()
    {
        audioSettings.Load();
        videoSettings.Load();
        controlSettings.Load();
    }

    void ShowSettings()
    {
        GameManager.PauseGame();
        HideTarget();
        Unlocked?.Invoke();
        optionsMenu.SetActive(true);
        settingsOpen = true;
    }

    public void HideSettings()
    {
        GameManager.ResumeGame();
        ShowTarget();
        Locked?.Invoke();
        optionsMenu.SetActive(false);
        settingsOpen = false;
    }

    public void ShowTarget()
    {
        targetReticle.SetActive(true);
    }

    void HideTarget()
    {
        targetReticle.SetActive(false);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // freeze cursor on screen centre
        Cursor.visible = false; // invisible cursor
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetMusicVolume(float value)
    {
        audioSettings.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        audioSettings.SetSfxVolume(value);
    }

    public void SetMouseSensitivity(float value)
    {
        controlSettings.SetMouseSensitivity(value);
    }
}
