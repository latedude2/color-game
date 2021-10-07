using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinePoint
{
    Vector3 position;
    LayerMask blockingLayers;
    public ShinePoint(Vector3 position){
        this.position = position;
        this.blockingLayers = 0b_0000_1001; //Block rays with default and static layers
    }
    
    public bool Reached(GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(position, lightPos, Color.red);
        }
        bool nothingIsBlockingLight = !Physics.Linecast(position, lightPos, blockingLayers);
        return nothingIsBlockingLight;
    }

    public Vector3 GetPosition()
    {
        return position;
    }
}
