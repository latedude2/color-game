using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseCollisionAudio : MonoBehaviour
{
    public FMODUnity.EventReference shatterEvent;
    public FMODUnity.EventReference collideEvent;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void PlayShatter(float force, float mass) {
        PlayOneShot(FMODUnity.RuntimeManager.CreateInstance(shatterEvent), force, mass);
    }

    public void PlayCollision(float force, float mass) {
        PlayOneShot(FMODUnity.RuntimeManager.CreateInstance(collideEvent), force, mass);
    }

    private void PlayOneShot(FMOD.Studio.EventInstance eventInstance, float force, float mass) {
        eventInstance.setParameterByName("Velocity Y", force);
        eventInstance.setParameterByName("Mass", mass);
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        eventInstance.start();
        eventInstance.release();
    }

    private void OnCollisionEnter(Collision collision) {
        PlayCollision(collision.impulse.magnitude / 50, rb.mass);
    }
}
