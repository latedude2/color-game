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
    private GameObject targetReticle;

    private void Start() {
        optionsMenu = transform.Find("Options").gameObject;
        targetReticle = transform.Find("TargetReticle").gameObject;
        Locked += LockCursor;
        Unlocked += UnlockCursor;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.M))
        {
            if(settingsOpen)
            {
                ShowTarget();
                HideSettings();
            }
            else
            {
                HideTarget();
                ShowSettings();
            }
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
