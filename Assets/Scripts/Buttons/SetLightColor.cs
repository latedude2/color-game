using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightColor : MonoBehaviour, Interactable
{
    private Light lightToSwitch;
    [SerializeField]
    private ColorCode lightColorToSet;
    private void Start() {
        lightToSwitch = transform.parent.GetComponentInChildren<Light>();
    }

    public void interact()
    {
        if(!lightToSwitch.enabled)
            lightToSwitch.enabled = true;
        transform.parent.GetComponentInChildren<ColoredLight>().SetColor(lightColorToSet);
    }
}
