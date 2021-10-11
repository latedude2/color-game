using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static Color GetColor(ColorCode colorCode)
    {
        Color color = new Color();
        switch (colorCode)
        {
            case ColorCode.Black:
                color = UnityEngine.Color.black;
                break;
            case ColorCode.Red:
                color = UnityEngine.Color.red;
                break;
            case ColorCode.Green:
                color = UnityEngine.Color.green;
                break;
            case ColorCode.Blue:
                color = UnityEngine.Color.blue;
                break;
            case ColorCode.Yellow:
                color = new UnityEngine.Color(1,1,0,1);
                break;
            case ColorCode.Magenta:
                color = UnityEngine.Color.magenta;
                break;
            case ColorCode.Cyan:
                color = UnityEngine.Color.cyan;
                break;
            case ColorCode.White:
                color = UnityEngine.Color.white;
                break;
            default:
                break;
        }
        return color;   
    }
}
