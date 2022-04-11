using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public struct OptimizedLight{
        public GameObject gameobject;
        public Transform transform;
        public Light lightComponent;
        public ColoredLight coloredLightComponent;
        public bool pointing;
    }
    public static OptimizedLight[] optimizedLights;
    void Start()
    {
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        optimizedLights = new OptimizedLight[lights.Length];
        for(int i = 0; i < optimizedLights.Length; i++)
        {
            optimizedLights[i].gameobject = lights[i];
            optimizedLights[i].pointing = false;
            optimizedLights[i].lightComponent = lights[i].GetComponent<Light>();
            optimizedLights[i].coloredLightComponent  = lights[i].GetComponent<ColoredLight>();
            optimizedLights[i].transform = lights[i].transform;
        }
    }

    public OptimizedLight[] GetLights()
    {
        return optimizedLights;
    }
}
