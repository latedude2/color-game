using UnityEngine;

public class SwitchLightButton : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    private AudioSource _audio;
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
        _audio = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
    }

    public void Interact()
    {
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
        _audio.PlayOneShot(_audio.clip);
    }

    private void initializeAudio() {

    }
}
