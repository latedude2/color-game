using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLight : MonoBehaviour
{
    [SerializeField] private bool enabledAtStart = false;
    [SerializeField] private ColorCode color = ColorCode.White;
    // Start is called before the first frame update
    void Start()
    {
        SetColor(color);
        Invoke(nameof(SetInitialEnabled), 0.1f);
    }

    void SetInitialEnabled()
    {
        gameObject.SetActive(enabledAtStart);
    }

    public void SetColor(ColorCode newColor)
    {
        color = newColor;
        Light light = gameObject.GetComponent<Light>();
        light.color = ColorHelper.GetColor(newColor);
    }

    public ColorCode GetColorCode()
    {
        return color;
    }
}
