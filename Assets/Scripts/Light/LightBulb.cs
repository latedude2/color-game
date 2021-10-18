using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    private Renderer bulbRenderer;
    private Material offMat;
    private Material onMat;
    private ColorCode color = ColorCode.Black;

    private void OnEnable() {
        bulbRenderer = GetComponent<Renderer>();
        offMat = Resources.Load<Material>("Materials/StaticEnvironment");
        bulbRenderer.material = offMat;
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            bulbRenderer.material = onMat;
        }
        else
        {
            bulbRenderer.material = offMat;
        }
    }

    public void SetColor(ColorCode newColor)
    {
        if (newColor == ColorCode.Black)
        {
            bulbRenderer.material = offMat;
        }
        else
        {
            onMat = Resources.Load<Material>("Materials/" + newColor.ToString());
            bulbRenderer.material = onMat;
        }
        
        color = newColor;
    }
}
