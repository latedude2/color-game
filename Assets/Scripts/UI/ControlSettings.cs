using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSettings : Loadable
{
    public static float mouseSensitivityMultiplier = 1f;
    public void SetMouseSensitivity(float value)
    {
        mouseSensitivityMultiplier = value;
        PlayerPrefs.SetFloat("mouseSensitivity", value);
    }


    public void Load()
    {
        SetMouseSensitivity(PlayerPrefs.GetFloat("mouseSensitivity"));
    }
}
