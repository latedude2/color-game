using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    private Transform[] shinePoints;
    private LightManager lightManager;

    void Start()
    {
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        shinePoints = FindShinePoints();
    }

    void FixedUpdate()
    {
        //TODO: Create list of points of the cube where we check if the light is shining. 

        //TODO: Raycast from the lights to the points and check if any of them reach the points without being occluded. 
        
    }

    Transform[] FindShinePoints()
    {
        List<Transform> shinepoints = new List<Transform>();
        //TODO: Create a list of points for the visible object that we will be checking for shine.
        return shinepoints.ToArray(); 
    }

    GameObject[] FindPointingLights()
    {
        //TODO: Create list of lights that are on and pointing towards the points, so we don't waste computations. 
        //Based on the normal vectors, only three sides of a box should be lit by the same light at once. 
        return lightManager.GetLights();
    }
}
