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
        switch (newColor)
        {
            case ColorCode.Black:
                light.color = UnityEngine.Color.black;
                break;
            case ColorCode.Red:
                light.color = UnityEngine.Color.red;
                break;
            case ColorCode.Green:
                light.color = UnityEngine.Color.green;
                break;
            case ColorCode.Blue:
                light.color = UnityEngine.Color.blue;
                break;
            case ColorCode.Yellow:
                light.color = new UnityEngine.Color(1,1,0,1);
                break;
            case ColorCode.Magenta:
                light.color = UnityEngine.Color.magenta;
                break;
            case ColorCode.Cyan:
                light.color = UnityEngine.Color.cyan;
                break;
            case ColorCode.White:
                light.color = UnityEngine.Color.white;
                break;
            default:
                break;
        }
    }

    public ColorCode GetColorCode()
    {
        return color;
    }
}
