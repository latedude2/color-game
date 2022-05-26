using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour
{
    private float shatterVelocity = 5f;
    [SerializeField] private GameObject replacementObject;
    private void OnCollisionEnter(Collision collision) {
        if(collision.relativeVelocity.magnitude > shatterVelocity)
        {
            GameObject go = Instantiate(replacementObject, transform.position, transform.rotation);
            go.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            Destroy(gameObject);
        }
    }
}
