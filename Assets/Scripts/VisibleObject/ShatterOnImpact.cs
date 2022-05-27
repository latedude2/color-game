using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour
{
    private float shatterImpulse = 50f;
    [SerializeField] private GameObject replacementObject;
    private VaseCollisionAudio audio;
    
    private void Start() {
        audio = GetComponent<VaseCollisionAudio>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.impulse.magnitude > shatterImpulse)
        {
            GameObject go = Instantiate(replacementObject, transform.position, transform.rotation);
            go.transform.localScale = transform.localScale;
            go.GetComponentInChildren<Rigidbody>().velocity = GetComponentInChildren<Rigidbody>().velocity;
            audio.PlayShatter(collision.impulse.magnitude / shatterImpulse);
            Destroy(gameObject);
        } else {
            audio.PlayCollision(collision.impulse.magnitude / shatterImpulse);
        }
    }
}
