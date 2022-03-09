using UnityEngine;
using FMODUnity;

public class SwitchLightButton : MonoBehaviour, Interactable
{
    public delegate void Interacted();
    public event Interacted OnInteract;
    private GameObject lightGameObject;
    // private StudioEventEmitter _audio;
    private EmittingSurface lightBulb;
    private Animation anim;

    Activatable[] activatableComponents;

    private void Start() {
        
        activatableComponents = transform.parent.GetComponentsInChildren<Activatable>();
        if(activatableComponents == null)
        {
            Debug.LogError("Button does not have activatable item connected");
        }
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        lightBulb = transform.parent.GetComponentInChildren<EmittingSurface>();
        // _audio = GetComponent<StudioEventEmitter>();
        anim = GetComponent<Animation>();
    }

    public void Interact()
    {
        if (!enabled)
            return;
        
        bool isActive = lightGameObject.activeInHierarchy;
        if(isActive)
        {
            foreach (Activatable activatable in activatableComponents)
            {
                activatable.Deactivate();
            }
        }
        else {
            foreach (Activatable activatable in activatableComponents)
            {
                activatable.Activate();
            }
        }
        anim.Play("ButtonAnimation");
        // _audio.Play();
        if (OnInteract != null) {
            OnInteract();
        }
    }
}
