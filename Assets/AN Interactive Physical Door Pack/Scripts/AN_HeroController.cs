using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_HeroController : MonoBehaviour
{
    [Tooltip("Character settings (rigid body)")]
    public float MoveSpeed = 4f, RunSpeed = 8f, JumpForce = 200f, Sensitivity = 70f;
    bool jumpFlag = true; // to jump from surface only

    CharacterController character;
    Rigidbody rb;
    Vector3 moveVector;

    Transform Cam;
    float yRotation;
    bool controlsEnabled = true;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cam = Camera.main.GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked; // freeze cursor on screen centre
        Cursor.visible = false; // invisible cursor
        #if UNITY_EDITOR
            Sensitivity = Sensitivity * 5;
        #endif
        Settings.Locked += EnableControls;
        Settings.Unlocked += DisableControls;
    }

    void Update()
    {
        if(controlsEnabled)
        {
            // camera rotation
            float xmouse = Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity * Settings.mouseSensitivityMultiplier;
            float ymouse = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity * Settings.mouseSensitivityMultiplier;
            transform.Rotate(Vector3.up * xmouse);
            yRotation -= ymouse;
            yRotation = Mathf.Clamp(yRotation, -85f, 60f);
            Cam.localRotation = Quaternion.Euler(yRotation, 0, 0);

            if (Input.GetButtonDown("Jump") && jumpFlag == true) rb.AddForce(transform.up * JumpForce);
        }
    }

    void FixedUpdate()
    {
        if(controlsEnabled)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveVector = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized * RunSpeed + transform.up * rb.velocity.y;
            }
            else 
            {
                moveVector = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized * MoveSpeed + transform.up * rb.velocity.y;
            }            
            rb.velocity = moveVector;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        jumpFlag = true; // hero can jump
    }

    private void OnTriggerExit(Collider other)
    {
        jumpFlag = false;
    }

    void EnableControls()
    {
        controlsEnabled = true;
    }

    void DisableControls()
    {
        controlsEnabled = false;
    }

}
