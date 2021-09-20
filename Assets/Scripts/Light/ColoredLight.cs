using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLight : MonoBehaviour
{
    
    [SerializeField] private Color color = Color.White;
    // Start is called before the first frame update
    void Start()
    {
        SetColor(color);
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        Light light = gameObject.GetComponent<Light>();
        switch (newColor)
        {
            case Color.Black:
                light.color = UnityEngine.Color.black;
                break;
            case Color.Red:
                light.color = UnityEngine.Color.red;
                break;
            case Color.Green:
                light.color = UnityEngine.Color.green;
                break;
            case Color.Blue:
                light.color = UnityEngine.Color.blue;
                break;
            case Color.Yellow:
                light.color = new UnityEngine.Color(1,1,0,1);
                break;
            case Color.Magenta:
                light.color = UnityEngine.Color.magenta;
                break;
            case Color.Cyan:
                light.color = UnityEngine.Color.cyan;
                break;
            case Color.White:
                light.color = UnityEngine.Color.white;
                break;
            default:
                break;
        }
    }
}
