using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ColorAdjustment : MonoBehaviour
{
    Volume vol;
    ColorAdjustments colorAdjustments;
    // Start is called before the first frame update
    void Start()
    {
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out colorAdjustments);
    }

    public void SetContrast(float value)
    {
        colorAdjustments.contrast.value = value;
    }

    public void SetBrightness(float value)
    {
        colorAdjustments.colorFilter.value = new Color(value, value, value);
    }
}
