using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinePoint
{
    Vector3 position;
    LayerMask blockingLayers;
    public ShinePoint(Vector3 position){
        this.position = position;
        this.blockingLayers = 0b_0001_0000_1011; //Block rays with default and static layers
    }
    
    public bool Reached(GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(position, lightPos, Color.red);
        }
        bool nothingIsBlockingLight = !Physics.Linecast(lightPos, position, blockingLayers);
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
