using UnityEngine;
using UnityEngine.AI;
using System;

public enum ObstacleMode
{
    Automatic = 0,
    ForceWall = 1,
    ForceBridge = 2
}

[ExecuteAlways]
public class VisibleObjectObstacle : MonoBehaviour, Activatable
{
    NavMeshObstacle obstacle;
    bool bridgeObstacle = false;

    [Tooltip("How to treat the obstacle. Automatic will decide based on existence of rigidbody and scale of object.")]
    [SerializeField] ObstacleMode forceObstacleMode = ObstacleMode.Automatic;
    void Start()
    {
        if(TryGetComponent<NavMeshObstacle>(out NavMeshObstacle newObstacle))
        {
            this.obstacle = newObstacle;
        }
        else
        {
            this.obstacle = gameObject.AddComponent<NavMeshObstacle>();
        }

        obstacle.carving = true;
        var sizeInWorld = GetComponent<Collider>().bounds.size;
        //TODO: Check for no rigidbody
        if(forceObstacleMode == ObstacleMode.ForceBridge || (forceObstacleMode == ObstacleMode.Automatic && sizeInWorld.y < sizeInWorld.x && sizeInWorld.y < sizeInWorld.z))   
        {
            ChangeToBridgeObstacle();
        }
    }

    void ChangeToBridgeObstacle()
    {
        Vector3 up = transform.InverseTransformDirection(Vector3.up);
        bridgeObstacle = true;
        obstacle.size = new Vector3(1,1,1) + Abs(up);
        obstacle.center = up * 1.5f;
    }
    Vector3 Abs(Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }


    public void BlockPath(bool enabled)
    {
        if(bridgeObstacle)
            obstacle.enabled = !enabled;
        obstacle.enabled = enabled;
    }


    public void Activate()
    {
        BlockPath(true);
    }

    public void Deactivate()
    {
        BlockPath(false);
    }
}
