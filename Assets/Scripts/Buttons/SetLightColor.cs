using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightColor : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    [SerializeField] private ColorCode lightColorToSet;

    private void Start() {
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
    }

    public void interact()
    {
        if(!lightGameObject.activeInHierarchy)
            lightGameObject.SetActive(true);
        transform.parent.GetComponentInChildren<ColoredLight>().SetColor(lightColorToSet);
    }
}
