using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_HeroController : MonoBehaviour
{
    [Tooltip("Character settings (rigid body)")]
    public float MoveSpeed = 4f, RunSpeed = 8f, JumpForce = 200f, Sensitivity = 70f;

    float enableControlSceneStartDelay = 0.5f;
    CharacterController character;
    Rigidbody rb;
    Vector3 moveVector;

    Transform Cam;
    float yRotation;
    bool controlsEnabled = true;
    CapsuleCollider m_Capsule;
    float groundCheckDistance = 0.6f;
    float stickToGroundHelperDistance = 0.5f;
    private Vector3 m_GroundContactNormal;


    private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


    void Start()
    {
        m_Capsule = GetComponent<CapsuleCollider>();
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
        DisableControls();
        Invoke(nameof(EnableControls), enableControlSceneStartDelay);
    }

    void Update()
    {
        if(controlsEnabled)
        {
            // camera rotation
            float xmouse = Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity * ControlSettings.mouseSensitivityMultiplier;
            float ymouse = Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity * ControlSettings.mouseSensitivityMultiplier;
            transform.Rotate(Vector3.up * xmouse);
            yRotation -= ymouse;
            yRotation = Mathf.Clamp(yRotation, -85f, 60f);
            Cam.localRotation = Quaternion.Euler(yRotation, 0, 0);

            if (Input.GetButtonDown("Jump") && m_IsGrounded && JumpForce > 0f) 
            {
                m_Jumping = true;
                rb.AddForce(transform.up * JumpForce);
            }
        }
    }

    void FixedUpdate()
    {
        GroundCheck();

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

        if (!m_IsGrounded)
            rb.drag = 0f;
        if (m_PreviouslyGrounded && !m_Jumping)
        {
            StickToGroundHelper();
        }
    }

    void EnableControls()
    {
        controlsEnabled = true;
    }

    void DisableControls()
    {
        controlsEnabled = false;
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius, Vector3.down, out hitInfo,
                                ((m_Capsule.height/2f) - m_Capsule.radius) +
                                stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                rb.velocity = Vector3.ProjectOnPlane(rb.velocity, hitInfo.normal);
            }
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius, Vector3.down, out hitInfo,
                                ((m_Capsule.height/2f) - m_Capsule.radius) + groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
        }
    }

}
