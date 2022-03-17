using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    ColorAdjustment colorAdjustment;
    private void Start() {
        colorAdjustment = GameObject.FindObjectsOfType<ColorAdjustment>()[0];
    }
    public void SetContrast(float value)
    {
        colorAdjustment.SetContrast(value);
        Debug.Log("Set brightness to: " + value);
    }

    public void SetBrightness(float value)
    {
        colorAdjustment.SetBrightness(value);
        Debug.Log("Set brightness to: " + value);
    }
}
