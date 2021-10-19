using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightColor : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    [SerializeField] private ColorCode lightColorToSet;
    private AudioClip onClip;
    private LightBulb lightBulb;
    private Animation anim;

    private void Start() {
        lightBulb = transform.parent.GetComponentInChildren<LightBulb>();
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        onClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOn");
        anim = GetComponent<Animation>();
    }

    public void interact()
    {
        lightGameObject.GetComponent<ColoredLight>().SetColor(lightColorToSet);
        lightBulb.SetColor(lightColorToSet);
        AudioSource.PlayClipAtPoint(onClip, GetComponent<Transform>().position);
        anim.Play("ButtonAnimation");

        if (!lightGameObject.activeInHierarchy)
        {
            lightGameObject.SetActive(true);
            lightBulb.SetActive(true);
        }
    }
}
