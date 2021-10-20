using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, Activatable
{
    [SerializeField] public Activatable gameObjectToActivate;

    private void Start() {
        if(gameObjectToActivate == null)
        {
            Debug.LogError("Light not set for light switch: " + this);
        }
    }
    public void activate()
    {
        gameObjectToActivate.activate();
    }

    public void deactivate()
    {
        gameObjectToActivate.deactivate();
    }
}
