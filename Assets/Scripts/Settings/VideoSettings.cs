using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour, Loadable
{
    ColorAdjustment colorAdjustment;
    float defaultContrast = 0;
    float defaultBrightness = 1;

    [SerializeField] Slider brightnessSlider;
    [SerializeField]  Slider contrastSlider;

    private void Awake() {
        colorAdjustment = GameObject.Find("PostProcessing").GetComponent<ColorAdjustment>();
    }
    public void SetContrast(float value)
    {
        contrastSlider.value = value;
        colorAdjustment.SetContrast(value);
        PlayerPrefs.SetFloat("contrast", value);
        PlayerPrefs.Save();
    }

    public void SetBrightness(float value)
    {
        brightnessSlider.value = value;
        colorAdjustment.SetBrightness(value);
        PlayerPrefs.SetFloat("brightness", value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        SetContrast(PlayerPrefs.GetFloat("contrast", defaultContrast));
        SetBrightness(PlayerPrefs.GetFloat("brightness", defaultBrightness));
    }
}
