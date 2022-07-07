using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinePoint
{
    Vector3 position;
    public ShinePoint(Vector3 position){
        this.position = position;
    }
    
    public bool Reached(Vector3 pointingLightPosition, LayerMask blockingLayers)
    {
        Vector3 lightPos = pointingLightPosition;
        
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(position, lightPos, Color.red);
        }
        
        bool nothingIsBlockingLight = !Physics.Linecast(lightPos, position, blockingLayers, QueryTriggerInteraction.Ignore);
        return nothingIsBlockingLight;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 newPosition)
    {
        this.position = newPosition;
    }
}
