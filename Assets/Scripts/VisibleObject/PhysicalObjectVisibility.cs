using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObjectVisibility : VisibleObjectVisibility
{
    [Tooltip("Optional material to use instead of the default black")]
    [SerializeField] private Material materialToUseInsteadOfBlack = null;

    override protected void SetColor(ColorCode color)
    {
        objectColor = color;
        if(color == ColorCode.Black && materialToUseInsteadOfBlack != null)
        {
            colorMat = materialToUseInsteadOfBlack;
        } else
        {
            colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        }
        _renderer.material = colorMat;
    }

    override protected void SetupBoundBox(){}
}
