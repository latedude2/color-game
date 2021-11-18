using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, Activatable
{
    [Tooltip("GameObject has to have component that implements the Activatable interface (ColoredLight or EmittingSurface for example).")]
    [SerializeField] GameObject[] gameObjectsToActivate;
    public bool active;
    private ParticleSystem sparks;
    private void Start() {
        if(gameObjectsToActivate.Length == 0)
        {
            Debug.LogError("Objects that should be activated was not set.");
        }
        sparks = GetComponentInChildren<ParticleSystem>();
    }
    public void Activate()
    {
        active = true;
        foreach(GameObject gameObjectToActivate in gameObjectsToActivate)
        {
            EmittingSurface emittingSurface = gameObjectToActivate.GetComponent<EmittingSurface>();
            if(emittingSurface != null)
            {
                emittingSurface.SetColor(GetComponent<VisibleObjectVisibility>().trueColor);
            }
            Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
            if(activatable != null)
                activatable.Activate();
            else
                Debug.LogError("Object is missing activatable component.");
        }
        sparks.Play();
    }

    public void Deactivate()
    {
        active = false;
        foreach(GameObject gameObjectToActivate in gameObjectsToActivate)
        {
            Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
            if(activatable != null)
                activatable.Deactivate();
            else
                Debug.LogError("Object is missing activatable component.");
        }
        sparks.Stop();
    }
}
