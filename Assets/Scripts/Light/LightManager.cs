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
}
