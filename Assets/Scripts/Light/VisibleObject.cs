using System.Collections.Generic;
using UnityEngine;
using Lightbug.GrabIt;

public class VisibleObject : MonoBehaviour
{
    public ColorCode color = ColorCode.Black;
    private Renderer _renderer;
    private Material colorMat;
    private Material blackMat;
    private Collider _collider;
    private Vector3[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    private GrabIt grabIt;
    [Tooltip("Draw the gizmos for the shine points at runtime. Used for debugging.")]
    public bool DisplayGizmos = false;
    private bool visible;

    Vector3 velocity;

    void Start()
    {
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _renderer = GetComponent<Renderer>();
        colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        blackMat = Resources.Load<Material>("Materials/Black");
        _collider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        shinePoints = FindShinePoints();
        // When object becomes lit and interactable
        if (isShinedOn())
        {
            // If object is not visible, make visible
            if (!visible)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().velocity = velocity;
                _renderer.material = colorMat;
                _collider.enabled = true;
                visible = true;
            }
        }
        else
        {
            // If object is visible, make invisible
            if (visible)
            {
                if (grabIt.m_targetRB != null && gameObject == grabIt.m_targetRB.gameObject)
                    grabIt.Drop();
                _renderer.material = blackMat;
                velocity = gameObject.GetComponent<Rigidbody>().velocity;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                _collider.enabled = false;
                visible = false;
            }
        }
    }

    private bool isShinedOn()
    {
        foreach (Vector3 point in shinePoints)
        {
            foreach (GameObject light in lightManager.GetPointingLights(point, color))
            {
                if (pointReached(point, light))
                {
                    return true;
                }
            }
        }
        return false;
    }

    Vector3[] FindShinePoints()
    {
        //Create a list of points for the visible object that we will be checking for shine.
        List<Vector3> shinepoints = new List<Vector3>();

        BoxCollider b = GetComponent<BoxCollider>();

        // Add points for each corner of the box collider to the list
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.51f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.51f));

        return shinepoints.ToArray();
    }

    void OnDrawGizmos()
    {
        if (DisplayGizmos)
        {

            Gizmos.color = Color.green;
            try
            {
                // Draw spheres for ShinePoints for debugging
                foreach (Vector3 point in shinePoints)
                    Gizmos.DrawSphere(point, .05f);
            }
            catch
            {
                new UnityException();
            }
        }
    }

    public bool pointReached(Vector3 point, GameObject pointingLight)
    {
        Vector3 lightPos = pointingLight.transform.position;
        if (DisplayGizmos)
        {
            Debug.DrawLine(point, lightPos, Color.red);
        }
        return !Physics.Linecast(point, lightPos);
    }

}
