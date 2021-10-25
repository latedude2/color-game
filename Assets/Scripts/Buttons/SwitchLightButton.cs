using UnityEngine;

public class SwitchLightButton : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    private AudioClip onClip;
    private AudioClip offClip;
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
        onClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOn");
        offClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOff");
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
        if (isActive) {
            AudioSource.PlayClipAtPoint(offClip, GetComponent<Transform>().position);  
        } else {
            AudioSource.PlayClipAtPoint(onClip, GetComponent<Transform>().position);
        }
    }

    private void initializeAudio() {

    }
}
