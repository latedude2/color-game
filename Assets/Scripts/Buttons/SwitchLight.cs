using UnityEngine;

public class SwitchLight : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    private AudioClip onClip;
    private AudioClip offClip;
    private LightBulb lightBulb;
    private Animation anim;

    private void Start() {
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        lightBulb = transform.parent.GetComponentInChildren<LightBulb>();
        onClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOn");
        offClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOff");
        anim = GetComponent<Animation>();
    }

    public void interact()
    {
        bool isActive = lightGameObject.activeInHierarchy;
        lightGameObject.SetActive(!isActive);
        lightBulb.SetActive(!isActive);
        anim["ButtonAnimation"].wrapMode = WrapMode.Once;
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
