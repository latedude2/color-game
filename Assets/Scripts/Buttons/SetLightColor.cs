using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightColor : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    [SerializeField] private ColorCode lightColorToSet;
    private AudioClip onClip;

    private void Start() {
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        onClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOn");
    }

    public void interact()
    {
        if(!lightGameObject.activeInHierarchy)
            lightGameObject.SetActive(true);
        transform.parent.GetComponentInChildren<ColoredLight>().SetColor(lightColorToSet);
        AudioSource.PlayClipAtPoint(onClip, GetComponent<Transform>().position);
    }
}
