using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightColor : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    [SerializeField] private ColorCode lightColorToSet;
    [SerializeField] private bool enablesLight = true;
    private AudioSource _audio;
    private EmittingSurface lightBulb;
    private Animation anim;
    Activatable[] activatableComponents;

    private void Start() {
        lightBulb = transform.parent.GetComponentInChildren<EmittingSurface>();
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        _audio = GetComponent<AudioSource>();
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
        _audio.PlayOneShot(_audio.clip);
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
