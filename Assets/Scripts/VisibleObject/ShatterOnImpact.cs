using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour
{
    private float shatterImpulse = 50f;
    private float minimalImpulseForSound = 2f;
    [SerializeField] private GameObject replacementObject;
    private VaseCollisionAudio audio;

    [SerializeField] private GameObject memoryPrefab;
    
    private void Start() {
        audio = GetComponent<VaseCollisionAudio>();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.impulse.magnitude > shatterImpulse)
        {
            GameObject go = Instantiate(replacementObject, transform.position, transform.rotation);
            go.transform.localScale = transform.localScale;
            go.GetComponentInChildren<Rigidbody>().velocity = GetComponentInChildren<Rigidbody>().velocity;
            audio.PlayShatter(collision.impulse.magnitude / shatterImpulse, GetComponent<Rigidbody>().mass);
            Destroy(gameObject);
            SpawnMemory();
        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        if(collision.impulse.magnitude > minimalImpulseForSound)
        {
            audio.PlayCollision(collision.impulse.magnitude / shatterImpulse, GetComponent<Rigidbody>().mass);
        }
    }

    void SpawnMemory()
    {
        if(TryGetComponent<MemoryInfo>(out MemoryInfo memoryInfo))
        {
            if(memoryInfo.memoryText != "")
            {
                GameObject newMemory = Instantiate(memoryPrefab, transform.position, transform.rotation);
                newMemory.GetComponent<Memory>().memoryDuration = memoryInfo.memoryDuration;
                newMemory.GetComponent<Memory>().memoryText = memoryInfo.memoryText;
            }
        }
    }
}
