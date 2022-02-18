using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class EmittingSurface : MonoBehaviour, Activatable
{
    private Renderer bulbRenderer;
    [SerializeField] private Material offMat;
    private Material onMat;
    private ColorCode color = ColorCode.Black;
    bool isActive = false;

    private void Start() {
        bulbRenderer = GetComponent<Renderer>();
        if(offMat == null)
            offMat = Resources.Load<Material>("Materials/Dark");    //Default material for script
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

    public void Activate()
    {
        SetActive(true);
    }

    public void Deactivate()
    {
        SetActive(false);
    }
}
