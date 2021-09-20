using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObject : MonoBehaviour
{
    private Transform[] shinePoints;    //A list of points of the box where we check if the box is hit by light
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
}
