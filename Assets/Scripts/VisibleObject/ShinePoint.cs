using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinePoint
{
    Vector3 position;
    LayerMask blockingLayers;
    GameObject visibleObject;
    public ShinePoint(Vector3 position, GameObject visibleObject){
        this.visibleObject = visibleObject;
        this.position = position;
        this.blockingLayers = 0b_0001_0000_1001; //Block rays with default, ignore outline and static layers
    }
    
    public bool Reached(GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        
        if (ColorGame.Debug.debugMode)
        {
            UnityEngine.Debug.DrawLine(position, lightPos, Color.red);
        }
        var layer = visibleObject.layer;
        visibleObject.layer = Physics.IgnoreRaycastLayer;
        bool nothingIsBlockingLight = !Physics.Linecast(lightPos, position, blockingLayers, QueryTriggerInteraction.Ignore);
        visibleObject.layer = layer;
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
