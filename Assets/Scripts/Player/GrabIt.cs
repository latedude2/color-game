using UnityEngine;

namespace Lightbug.GrabIt
{

[System.Serializable]
    public class GrabObjectProperties
    {

        public bool m_useGravity = false;
        public float m_drag = 10;
        public float m_angularDrag = 10;
        public RigidbodyConstraints m_constraints = RigidbodyConstraints.FreezeRotation;

    }

    public class GrabIt : MonoBehaviour
    {

        [Header("Input")]
        [SerializeField] KeyCode m_rotatePitchPosKey = KeyCode.I;
        [SerializeField] KeyCode m_rotatePitchNegKey = KeyCode.K;
        [SerializeField] KeyCode m_rotateYawPosKey = KeyCode.L;
        [SerializeField] KeyCode m_rotateYawNegKey = KeyCode.J;

        [Header("Grab properties")]

        [SerializeField]
        [Range(4, 50)]
        float m_grabSpeed = 7;

        [SerializeField]
        [Range(0.1f, 5)]
        float m_grabMinDistance = 1;

        [SerializeField]
        [Range(2, 25)]
        float m_grabMaxDistance = 10;

        [SerializeField]
        [Range(1, 10)]
        float m_scrollWheelSpeed = 5;

        [SerializeField]
        [Range(50, 500)]
        float m_angularSpeed = 300;

        [SerializeField]
        [Range(10, 50)]
        float m_impulseMagnitude = 25;

        public delegate void GrabObjectHandler();
        public static event GrabObjectHandler Grabbed;
        public static event GrabObjectHandler Released;


        [Header("Affected Rigidbody Properties")]
        [SerializeField] GrabObjectProperties m_grabProperties = new GrabObjectProperties();

        GrabObjectProperties m_defaultProperties = new GrabObjectProperties();

        [Header("Layers")]
        [SerializeField]
        LayerMask m_collisionMask;



        public Rigidbody m_targetRB = null;
        Transform m_transform;

        Vector3 m_targetPos;
        GameObject m_hitPointObject;
        float m_targetDistance;

        bool m_holding = false;
        bool m_applyImpulse = false;
        bool m_isHingeJoint = false;
        bool m_isConfigurableJoint = false;

        //Debug
        LineRenderer m_lineRenderer;



        void Awake()
        {
            m_transform = transform;
            m_hitPointObject = new GameObject("Point");

            m_lineRenderer = GetComponent<LineRenderer>();
        }


        void Update()
        {
            if (m_holding)
            {
                if (ColorGame.Debug.debugMode)
                    m_targetDistance += Input.GetAxisRaw("Mouse ScrollWheel") * m_scrollWheelSpeed;
                m_targetDistance = Mathf.Clamp(m_targetDistance, m_grabMinDistance, m_grabMaxDistance);

                m_targetPos = m_transform.position + m_transform.forward * m_targetDistance;

                if (ColorGame.Debug.debugMode)
                {
                    if (!m_isHingeJoint && !m_isConfigurableJoint)
                    {
                        if (Input.GetKey(m_rotatePitchPosKey) || Input.GetKey(m_rotatePitchNegKey) || Input.GetKey(m_rotateYawPosKey) || Input.GetKey(m_rotateYawNegKey))
                        {
                            m_targetRB.constraints = RigidbodyConstraints.None;
                        }
                        else
                        {
                            m_targetRB.constraints = m_grabProperties.m_constraints;
                        }
                    }
                }


                if (Input.GetMouseButtonUp(0))
                {
                    Release();
                    m_holding = false;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    m_applyImpulse = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TryToGrab();
                }
            }

        }

        private void TryToGrab()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(m_transform.position, m_transform.forward, out hitInfo, m_grabMaxDistance, m_collisionMask))
            {
                Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();
                if(rb == null && hitInfo.collider.transform.parent != null)
                {
                    rb = hitInfo.collider.transform.parent.GetComponent<Rigidbody>();
                }
                if (rb != null)
                {
                    SetHeldObject(rb, hitInfo.distance);
                    m_holding = true;
                    Grabbed?.Invoke();
                }
            }
        }

        void SetHeldObject(Rigidbody target, float distance)
        {
            m_targetRB = target;
            m_isHingeJoint = target.GetComponent<HingeJoint>() != null;
            m_isConfigurableJoint = target.GetComponent<ConfigurableJoint>() != null;

            //Rigidbody default properties	
            m_defaultProperties.m_useGravity = m_targetRB.useGravity;
            m_defaultProperties.m_drag = m_targetRB.drag;
            m_defaultProperties.m_angularDrag = m_targetRB.angularDrag;
            m_defaultProperties.m_constraints = m_targetRB.constraints;

            //Grab Properties	
            m_targetRB.useGravity = m_grabProperties.m_useGravity;
            m_targetRB.drag = m_grabProperties.m_drag;
            m_targetRB.angularDrag = m_grabProperties.m_angularDrag;
            if(m_isHingeJoint || m_isConfigurableJoint)
            {
                m_targetRB.constraints = RigidbodyConstraints.None;
            }
            else {
                m_targetRB.constraints = m_grabProperties.m_constraints;
            }

            m_hitPointObject.transform.SetParent(target.transform);

            m_targetDistance = distance;
            m_targetPos = m_transform.position + m_transform.forward * m_targetDistance;

            m_hitPointObject.transform.position = m_targetPos;
            m_hitPointObject.transform.LookAt(m_transform);

        }

        void Release()
        {
            //Grab Properties	
            m_targetRB.useGravity = m_defaultProperties.m_useGravity;
            m_targetRB.drag = m_defaultProperties.m_drag;
            m_targetRB.angularDrag = m_defaultProperties.m_angularDrag;
            m_targetRB.constraints = m_defaultProperties.m_constraints;

            m_targetRB = null;

            m_hitPointObject.transform.SetParent(null);

            if (m_lineRenderer != null)
                m_lineRenderer.enabled = false;

            Released?.Invoke();
        }

        void Hold()
        {
            Vector3 hitPointPos = m_hitPointObject.transform.position;
            Vector3 dif = m_targetPos - hitPointPos;

            if (m_isHingeJoint || m_isConfigurableJoint){
                if (m_targetRB.GetComponent<Lamp>() != null)
                {
                    Vector3 targetDirection = m_transform.transform.forward;
                    Quaternion targetrotation = Quaternion.LookRotation(targetDirection);
                    float turnspeed = m_targetRB.GetComponent<Lamp>().GetTurnSpeed();
                    m_targetRB.transform.rotation = Quaternion.RotateTowards(m_targetRB.transform.rotation, targetrotation, Time.fixedDeltaTime * turnspeed);
                }
                else
                    m_targetRB.AddForceAtPosition(m_grabSpeed * dif * 100, hitPointPos, ForceMode.Force);
            }
            else
                m_targetRB.velocity = m_grabSpeed * dif;

            if (Vector3.Distance(m_transform.position,m_targetRB.transform.position) > m_grabMaxDistance)
                Drop();

            if (m_lineRenderer != null)
            {
                m_lineRenderer.enabled = true;
                m_lineRenderer.SetPositions(new Vector3[] { m_targetPos, hitPointPos });
            }
        }

        public void Drop()
        {
            if(m_holding)
            {
                Release();
                m_holding = false;
                m_applyImpulse = false;  
            }          
        }

        void Rotate()
        {
            if (Input.GetKey(m_rotatePitchPosKey))
            {
                m_targetRB.AddTorque(m_transform.right * m_angularSpeed);
            }
            else if (Input.GetKey(m_rotatePitchNegKey))
            {
                m_targetRB.AddTorque(-m_transform.right * m_angularSpeed);
            }

            if (Input.GetKey(m_rotateYawPosKey))
            {
                m_targetRB.AddTorque(-m_transform.up * m_angularSpeed);
            }
            else if (Input.GetKey(m_rotateYawNegKey))
            {
                m_targetRB.AddTorque(m_transform.up * m_angularSpeed);
            }
        }

        void FixedUpdate()
        {
            if (!m_holding)
                return;

            if (!m_isHingeJoint && !m_isConfigurableJoint)
                Rotate();

            Hold();
            Throw();
        }

        private void Throw()
        {
            if (m_applyImpulse)
            {
                m_targetRB.velocity = m_transform.forward * m_impulseMagnitude;
                Release();
                m_holding = false;
                m_applyImpulse = false;
            }
        }

        public float GetGrabDistance()
		{
			return m_grabMaxDistance;
		}
    }
}
