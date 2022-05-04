using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClip : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    [SerializeField] float noClipSpeed = 10;

    float sprintMultiplier = 1;

    [SerializeField] private Transform cameraTransform;
    private bool noClipEnabled = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(ColorGame.Debug.debugMode)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!noClipEnabled)
                {
                    EnableNoClip();
                }
                else
                {
                    DisableNoClip();
                }
            }
            if(noClipEnabled)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                    sprintMultiplier = 2;
                else sprintMultiplier = 1;

                if (Input.GetKey(KeyCode.W))
                {
                    transform.position += cameraTransform.forward * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    transform.position -= cameraTransform.forward * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.position += cameraTransform.right * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.position -= cameraTransform.right * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
                if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
                {
                    transform.position += transform.up * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
                if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftControl))
                {
                    transform.position -= transform.up * Time.deltaTime * noClipSpeed * sprintMultiplier;
                }
            }
        }
    }
    void EnableNoClip()
    {
        noClipEnabled = true;
        rb.isKinematic = true;
    }
    void DisableNoClip()
    {
        noClipEnabled = false;
        rb.isKinematic = false;
    }
}

