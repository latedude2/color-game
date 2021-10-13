using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private static GameObject[] lights;
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
    }

    public GameObject[] GetLights()
    {
        return lights;
    }

    public static GameObject[] GetPointingLights(Vector3 point, ColorCode color)
    {
        List<GameObject> pointingLights = new List<GameObject>();
        foreach (GameObject light in lights)
        {
            if(!light.activeInHierarchy)
                continue;
            //If the colored object cannot reflect the light. We check for bitwise overlap here.
            if((color & light.GetComponent<ColoredLight>().GetColorCode()) == 0)
            {
                continue;
            }
            //If the light is pointing towards the point
            if (Vector3.Angle(light.transform.forward, point - light.transform.position) < light.GetComponent<Light>().spotAngle / 2)
            {
                pointingLights.Add(light);
            }
        }
        //Possible Optimization: Based on the normal vectors, only three sides of a box should be lit by the same light at once. 
        return pointingLights.ToArray();
    }
}
