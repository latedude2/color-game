using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour
{
    private float shatterImpulse = 50f;
    [SerializeField] private GameObject replacementObject;
    private void OnCollisionEnter(Collision collision) {
        if(collision.impulse.magnitude > shatterImpulse)
        {
            GameObject go = Instantiate(replacementObject, transform.position, transform.rotation);
            go.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            Destroy(gameObject);
        }
    }
}
