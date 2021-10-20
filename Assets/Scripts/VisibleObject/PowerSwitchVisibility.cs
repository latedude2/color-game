using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchVisibility : VisibleObjectVisibility
{
    
    override protected void SetVisibility(ColorCode objectFinalColor)
    {
        if (objectFinalColor.HasFlag(trueColor))
        {
            if (objectFinalColor != color)
                SetColor(objectFinalColor);
            // If object is not visible, make visible
            if (!visible)
            {
                SetToVisible();
            }
        }
        else
        {
            // If object is visible, make invisible
            if (visible)
            {
                SetColor(ColorCode.Black);
                SetToInvisible();
            }
        }
    }
}
