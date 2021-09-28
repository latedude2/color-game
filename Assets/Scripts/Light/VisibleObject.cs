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
    private bool visible = false;

    Vector3 velocity;

    void Start()
    {
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        grabIt = GameObject.Find("Main Camera").GetComponent<GrabIt>();
        _renderer = GetComponent<Renderer>();
        colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        blackMat = Resources.Load<Material>("Materials/Black");
        _renderer.material = blackMat;
        _collider = GetComponent<Collider>();
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        _collider.enabled = true;
    }

    void FixedUpdate()
    {
        shinePoints = FindShinePoints();
        // When object becomes lit and interactable
        if (isShinedOn())
        {
            // If layer is not "Default", set to "Default"
            if (gameObject.layer != 0)
            {
                gameObject.layer = 0;
            }
        }
        else
        {
            // If layer is not "Non-interactable", set to "Non-interactable"
            if (gameObject.layer != 7)
            {
                gameObject.layer = 7;
                grabIt.Drop();
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
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f));
        shinepoints.Add(transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f));

        return shinepoints.ToArray();
    }

    void OnDrawGizmos()
    {
        if (ColorGame.Debug.debugMode)
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
        Vector3 direction = lightPos - point; //direction from point to the light
        float dist = Vector3.Distance(lightPos, point);
        return Physics.Raycast(point, direction, dist);
    }

}
