using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, Activatable
{
    [Tooltip("GameObject has to have component that implements the Activatable interface (ColoredLight or EmittingSurface for example).")]
    [SerializeField] GameObject[] gameObjectsToActivate;
    private void Start() {
        if(gameObjectsToActivate.Length == 0)
        {
            Debug.LogError("Objects that should be activated was not set.");
        }
    }
    public void Activate()
    {
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
    }

    public void Deactivate()
    {
        foreach(GameObject gameObjectToActivate in gameObjectsToActivate)
        {
            Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
            if(activatable != null)
                activatable.Deactivate();
            else
                Debug.LogError("Object is missing activatable component.");
        }
    }
}
