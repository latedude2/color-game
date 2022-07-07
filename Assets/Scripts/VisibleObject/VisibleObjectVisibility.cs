using System.Collections.Generic;
using System;
using UnityEngine;
using DimBoxes;
using UnityEngine.Events;

public class VisibleObjectVisibility : MonoBehaviour
{
    protected ColorCode objectColor = ColorCode.Black;
    public ColorCode trueColor = ColorCode.Black;
    protected Renderer _renderer;
    protected Material colorMat;
    private Material blackMat;
    protected BoundBox boundBox;
    private ShinePoint[] shinePoints;    //A list of points of the box where we check if the box is hit by light
    private LightManager lightManager;
    public bool visible = false;
    [SerializeField] [Range(1, 5)] int shinePointMultiplier = 1;
    Activatable[] activatableComponents;
    
    public UnityEvent visibilityChanged;
    Collider _collider;

    Rigidbody rigidbody = null;
    Enemy enemy = null;
    
    void Start()
    {
        TryGetComponent<Collider>(out _collider);
        if(_collider == null)
        {
            Transform child = transform.GetChild(0);
            if(child != null)
            {
                child.TryGetComponent<Collider>(out _collider);
            }
        }
        rigidbody = GetComponent<Rigidbody>();
        enemy = GetComponent<Enemy>();
        activatableComponents = GetComponents<Activatable>();
        lightManager = GameObject.Find("LightManager").GetComponent<LightManager>();
        _renderer = GetComponent<Renderer>();
        if(trueColor == ColorCode.Black)
        {
            trueColor = (ColorCode) Enum.Parse(typeof(ColorCode), _renderer.material.name.Replace("(Instance)",""));
        }
        blackMat = Resources.Load<Material>("Materials/Black");
        _renderer.material = blackMat;
        SetupBoundBox();
        
        shinePoints = CreateShinePoints();
        SetColor(ColorCode.Black);
        SetToInvisible();
    }

    void FixedUpdate()
    {
        if(rigidbody != null || enemy != null)
        {
            UpdateShinePoints();
        }
        // When object becomes lit and interactable
        ColorCode objectFinalColor = FindShownColor();
        //Check object should be visible
        SetVisibility(objectFinalColor);
    }

    protected virtual void SetVisibility(ColorCode objectFinalColor)
    {
        if (objectFinalColor != ColorCode.Black)
        {
            if (objectFinalColor != objectColor)
            {
                SetColor(objectFinalColor);
            }
            // If object is not visible, make visible
            if (!visible)
            {
                SetToVisible();
            }
        }
        else
        {
            // If object is visible, make invisible
            if (visible)
            {
                SetColor(ColorCode.Black);
                SetToInvisible();
            }
        }
    }

    protected void SetToVisible()
    {
        foreach (Activatable activatable in activatableComponents)
        {
            activatable.Activate();
        }
        visible = true;
        visibilityChanged.Invoke();
    }

    protected void SetToInvisible()
    {
        foreach (Activatable activatable in activatableComponents)
        {
            activatable.Deactivate();
        }
        visible = false;
        visibilityChanged.Invoke();
    }

    protected virtual void SetupBoundBox()
    {
        boundBox = GetComponent<BoundBox>();
        boundBox.lineColor = ColorHelper.GetColor(trueColor);
        boundBox.SetLineRenderers();
    }

    protected virtual void SetColor(ColorCode color)
    {
        objectColor = color;
        colorMat = Resources.Load<Material>("Materials/" + color.ToString());
        _renderer.material = colorMat;

        if (color == trueColor)
        {
            boundBox.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            boundBox.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Ignore Outline");
        }
    }

    private ColorCode FindShownColor()
    {
        ColorCode finalColor = ColorCode.Black;
        var gameObjectLayer = gameObject.layer;
        gameObject.layer = 0b_0000_0010;    //We ignore the object itself since we assume the visible object will always be concave
        if(trueColor.HasFlag(ColorCode.Green) && IsShinedByColor(ColorCode.Green))
        {
            finalColor = finalColor | ColorCode.Green;
        }
        if(trueColor.HasFlag(ColorCode.Red) && IsShinedByColor(ColorCode.Red))
        {
            finalColor = finalColor | ColorCode.Red;
        }
        if(trueColor.HasFlag(ColorCode.Blue) && IsShinedByColor(ColorCode.Blue))
        {
            finalColor = finalColor | ColorCode.Blue;
        }
        if(finalColor == (ColorCode.Green | ColorCode.Red | ColorCode.Blue))
            finalColor = ColorCode.White;
        gameObject.layer = gameObjectLayer; //set back the original layer
        return finalColor;
    }

    private bool IsShinedByColor(ColorCode color)
    {
        foreach (ShinePoint shinePoint in shinePoints)
        {
            for(int i = 0; i < LightManager.optimizedLights.Length; i++)
            {
                if (IsPointing(LightManager.optimizedLights[i], color, shinePoint))
                {
                    if (shinePoint.Reached(LightManager.optimizedLights[i].gameobject))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsPointing(LightManager.OptimizedLight pointingLight, ColorCode color, ShinePoint shinePoint)
    {
        if(!pointingLight.gameobject.activeInHierarchy)
        {
            return false;
        }
        //If the colored object cannot reflect the light. We check for bitwise overlap here.
        if((color & pointingLight.coloredLightComponent.GetColorCode()) == 0)
        {
            return false;
        }
        //If the light is pointing towards the point
        if (Vector3.Angle(pointingLight.transform.forward, shinePoint.GetPosition() - pointingLight.transform.position) < pointingLight.lightComponent.spotAngle / 2)
        {
            return true;
        }
        return false;
    }

    ShinePoint[] CreateShinePoints()
    {
        //Create a list of points for the visible object that we will be checking for shine.
        List<ShinePoint> shinepoints = new List<ShinePoint>();
        if(_collider is BoxCollider)
        {
            return CreateBoxShinePoints((BoxCollider)_collider, shinepoints);
        }
        else  return CreateMeshShinePoints((MeshCollider)_collider, shinepoints);   
    }

    ShinePoint[] CreateBoxShinePoints(BoxCollider boxCollider, List<ShinePoint> shinepoints)
    {
        // Add points on the box collider to the list
        for (int z = -shinePointMultiplier; z <= shinePointMultiplier; z++)
        {
            for (int y = -shinePointMultiplier; y <= shinePointMultiplier; y++)
            {
                for (int x = -shinePointMultiplier; x <= shinePointMultiplier; x++)
                {
                    // skip the middle of the box
                    if (!IsShinePointInMiddle(x, y, z, shinePointMultiplier))
                        shinepoints.Add(new ShinePoint(transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x * x / shinePointMultiplier, boxCollider.size.y * y / shinePointMultiplier, boxCollider.size.z * z / shinePointMultiplier) * 0.50f), gameObject));
                }
            }
        }
        return shinepoints.ToArray();
    }

    ShinePoint[] CreateMeshShinePoints(MeshCollider collider, List<ShinePoint> shinepoints)
    {
        Mesh mesh = collider.sharedMesh;
        foreach (var vertice in mesh.vertices) {
            shinepoints.Add(new ShinePoint(transform.TransformPoint(vertice), gameObject));
        }
        return shinepoints.ToArray();
    }

    void UpdateShinePoints()
    {
        if (_collider is MeshCollider) {
            UpdateMeshShinePoints((MeshCollider)_collider);
        }
        else {
            UpdateBoxShinePoints((BoxCollider)_collider);
        }
    }

    void UpdateMeshShinePoints(MeshCollider collider)
    {
        Mesh mesh = collider.sharedMesh;
        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            shinePoints[i].SetPosition(transform.TransformPoint(mesh.vertices[i]));
        }
    }

    void UpdateBoxShinePoints(BoxCollider boxCollider)
    {
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center; 
        int i = 0;
        // Add points on the box collider to the list
        for (int z = -shinePointMultiplier; z <= shinePointMultiplier; z++)
        {
            for (int y = -shinePointMultiplier; y <= shinePointMultiplier; y++)
            {
                for (int x = -shinePointMultiplier; x <= shinePointMultiplier; x++)
                {
                    // skip the middle of the box
                    if (!IsShinePointInMiddle(x, y, z, shinePointMultiplier))
                    {
                        shinePoints[i].SetPosition(transform.TransformPoint(center + new Vector3(size.x * x / shinePointMultiplier, size.y * y / shinePointMultiplier, size.z * z / shinePointMultiplier) * 0.50f));
                        i++;
                    }
                }
            }
        }
    }

    bool IsShinePointInMiddle(int x, int y, int z, int shinePointMultiplier)
    {
        if(x == shinePointMultiplier || x == -shinePointMultiplier)
            return false;
        if(y == shinePointMultiplier || y == -shinePointMultiplier)
            return false;
        if(z == shinePointMultiplier || z == -shinePointMultiplier)
            return false;
        return true;
    }

    void OnDrawGizmos()
    {
        if (ColorGame.Debug.debugMode)
        {
            Gizmos.color = Color.yellow;
            try
            {
                // Draw spheres for ShinePoints for debugging
                foreach (ShinePoint point in shinePoints)
                {
                    Gizmos.DrawSphere(point.GetPosition(), .05f);
                }
            }
            catch
            {
                new UnityException();
            }
        }
    }
}