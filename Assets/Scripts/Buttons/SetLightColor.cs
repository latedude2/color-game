using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SetLightColor : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    [SerializeField] private ColorCode lightColorToSet;
    [SerializeField] private bool enablesLight = true;
    private StudioEventEmitter _audio;
    private EmittingSurface lightBulb;
    private Animation anim;
    Activatable[] activatableComponents;

    private void Start() {
        lightBulb = transform.parent.parent.parent.GetComponentInChildren<EmittingSurface>();
        lightGameObject = transform.parent.parent.parent.GetComponentInChildren<Light>().gameObject;
        _audio = GetComponent<StudioEventEmitter>();
        anim = GetComponent<Animation>();
        activatableComponents = transform.parent.GetComponentsInChildren<Activatable>();
        if(activatableComponents == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
    }

    public void Interact()
    {
        lightGameObject.GetComponent<ColoredLight>().SetColor(lightColorToSet);
        lightBulb.SetColor(lightColorToSet);
        _audio.Play();
        anim.Play("ButtonAnimation");

        if(enablesLight)
        {
            bool isActive = lightGameObject.activeInHierarchy;
            if(!isActive)
            {
                foreach (Activatable activatable in activatableComponents)
                {
                    activatable.Activate();
                }
            }
        }
    }

    
}
