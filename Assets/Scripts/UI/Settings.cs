using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public delegate void PointerHandler();
    public static event PointerHandler Locked;
    public static event PointerHandler Unlocked;

    private bool settingsOpen = false;
    public static float mouseSensitivityMultiplier = 1f;
    public static float soundEffectVolumeMultiplier = 1f;
    public static float musicVolumeMultiplierMultiplier = 1f;
    private GameObject optionsMenu;

    private void Start() {
        optionsMenu = transform.Find("Options").gameObject;
        Locked += LockCursor;
        Unlocked += UnlockCursor;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(settingsOpen)
                HideSettings();
            else
                ShowSettings();
        }
    }

    void OnDestroy() {
        //It is a good practice to unsubscribe events when the objects are destroyed to avoid errors.
        Locked -= LockCursor;
        Unlocked -= UnlockCursor;
    }

    void ShowSettings()
    {
        Unlocked?.Invoke();
        optionsMenu.SetActive(true);
        settingsOpen = true;
    }

    public void HideSettings()
    {
        Locked?.Invoke();
        optionsMenu.SetActive(false);
        settingsOpen = false;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // freeze cursor on screen centre
        Cursor.visible = false; // invisible cursor
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // freeze cursor on screen centre
        Cursor.visible = true; // invisible cursor
    }

    public void SetMusicVolume(float value)
    {
        musicVolumeMultiplierMultiplier = value;
    }

    public void SetSfxVolume(float value)
    {
        soundEffectVolumeMultiplier = value;
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivityMultiplier = value;
    }
}
