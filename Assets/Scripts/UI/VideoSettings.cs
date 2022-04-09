using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettings : MonoBehaviour, Loadable
{
    ColorAdjustment colorAdjustment;
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
        SetContrast(PlayerPrefs.GetFloat("contrast"));
        SetBrightness(PlayerPrefs.GetFloat("brightness"));
    }
}
