using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, Activatable
{
    [Tooltip("GameObject has to have component that implements the Activatable interface (ColoredLight for example).")]
    [SerializeField] GameObject gameObjectToActivate;

    private void Start() {
        if(gameObjectToActivate == null)
        {
            Debug.LogError("Object that should be activated was not set.");
        }
    }
    public void Activate()
    {
        Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
        if(activatable != null)
            activatable.Activate();
        else
            Debug.LogError("Object is missing activatable component.");
    }

    public void Deactivate()
    {
        Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
        if(activatable != null)
            activatable.Deactivate();
        else
            Debug.LogError("Object is missing activatable component.");
    }
}
