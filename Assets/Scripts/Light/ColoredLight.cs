using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLight : MonoBehaviour
{
    
    [SerializeField] private ColorCode color = ColorCode.White;
    // Start is called before the first frame update
    void Start()
    {
        SetColor(color);
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
