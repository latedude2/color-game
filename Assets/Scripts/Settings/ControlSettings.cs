using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour, Loadable
{
    [SerializeField] Slider sensitivitySlider;

    public static float mouseSensitivityMultiplier = 1f;
    float defaultSensitivity = 1;
    public void SetMouseSensitivity(float value)
    {
        sensitivitySlider.value = value;
        mouseSensitivityMultiplier = value;
        PlayerPrefs.SetFloat("mouseSensitivity", value);
        PlayerPrefs.Save();
    }


    public void Load()
    {
        SetMouseSensitivity(PlayerPrefs.GetFloat("mouseSensitivity", defaultSensitivity));
    }
}
