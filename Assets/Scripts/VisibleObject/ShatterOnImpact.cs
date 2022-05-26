using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour
{
    private float shatterImpulse = 50f;
    [SerializeField] private GameObject replacementObject;
    public FMODUnity.EventReference FMODEvent;
    private void OnCollisionEnter(Collision collision) {
        // GetComponent<FMODUnity.StudioEventEmitter>().Play();

        FMOD.Studio.EventInstance heal = FMODUnity.RuntimeManager.CreateInstance(FMODEvent);
        heal.setParameterByName("Velocity Y", collision.impulse.magnitude/50);
        Debug.Log(collision.impulse.magnitude/50);
        heal.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        heal.start();
        heal.release();

        if(collision.impulse.magnitude > shatterImpulse)
        {
            GameObject go = Instantiate(replacementObject, transform.position, transform.rotation);
            go.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            Destroy(gameObject);
        }
    }
}
