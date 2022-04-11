using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettings : MonoBehaviour, Loadable
{
    ColorAdjustment colorAdjustment;

    float defaultContrast = 0;
    float defaultBrightness = 1;

    private void Start() {
        colorAdjustment = GameObject.FindObjectsOfType<ColorAdjustment>()[0];
    }
    public void SetContrast(float value)
    {
        colorAdjustment.SetContrast(value);
        PlayerPrefs.SetFloat("contrast", value);
    }

    public void SetBrightness(float value)
    {
        colorAdjustment.SetBrightness(value);
        PlayerPrefs.SetFloat("brightness", value);
    }

    public void Load()
    {
        SetContrast(PlayerPrefs.GetFloat("contrast", defaultContrast));
        SetBrightness(PlayerPrefs.GetFloat("brightness", defaultBrightness));
    }
}
