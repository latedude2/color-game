using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : MonoBehaviour, Interactable
{
    private Light lightToSwitch;
    private void Start() {
        lightToSwitch = transform.parent.GetComponentInChildren<Light>();
    }

    public void interact()
    {
        lightToSwitch.enabled = !lightToSwitch.enabled;
    }
}
