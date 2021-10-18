using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulb : MonoBehaviour
{
    private Renderer bulbRenderer;
    private Material offMat;
    private Material onMat;
    private ColorCode color = ColorCode.Black;
    bool isActive = false;

    private void Start() {
        bulbRenderer = GetComponent<Renderer>();
        offMat = Resources.Load<Material>("Materials/Dark");
    }

    public void SetActive(bool activate)
    {
        bulbRenderer.material = activate ? onMat : offMat;
        isActive = activate;
    }

    public void SetColor(ColorCode newColor)
    {
        if (newColor != ColorCode.Black)
        {
            onMat = Resources.Load<Material>("Materials/" + newColor.ToString());
            if (isActive)
                bulbRenderer.material = onMat;
        }
        
        color = newColor;
    }
}
