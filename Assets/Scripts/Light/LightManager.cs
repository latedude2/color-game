using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private GameObject[] lights;
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
    }

    public GameObject[] GetLights()
    {
        return lights;
    }
    
    public GameObject[] GetPointingLights(Vector3 point)
    {
        List<GameObject> pointingLights = new List<GameObject>();
        //Optimization: Create list of lights that are on and pointing towards the points, so we don't waste computations. 
        foreach( GameObject light in lights)
        {
            //If the light is pointing towards the point
            if  ( Vector3.Angle(light.transform.forward, point - light.transform.position) < light.GetComponent<Light>().spotAngle) 
            {
                pointingLights.Add(light); 
            }
        }
        //Optimization: Based on the normal vectors, only three sides of a box should be lit by the same light at once. 
        return pointingLights.ToArray();
    }
}
