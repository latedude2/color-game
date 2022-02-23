using UnityEngine;
using UnityEngine.AI;
using System;

public enum ObstacleMode
{
    Automatic = 0,
    ForceWall = 1,
    ForceBridge = 2
}

public class VisibleObjectObstacle : MonoBehaviour
{
    NavMeshObstacle obstacle;
    bool bridgeObstacle = false;

    [Tooltip("How to treat the obstacle. Automatic will decide based on existence of rigidbody and scale of object.")]
    [SerializeField] ObstacleMode forceObstacleMode = ObstacleMode.Automatic;
    void Start()
    {
        obstacle = gameObject.AddComponent<NavMeshObstacle>();
        obstacle.carving = true;

        var sizeInWorld = GetComponent<Collider>().bounds.size;
        //TODO: Check for no rigidbody
        if(forceObstacleMode == ObstacleMode.ForceBridge || (forceObstacleMode == ObstacleMode.Automatic && sizeInWorld.y < sizeInWorld.x && sizeInWorld.y < sizeInWorld.z))   
        {
            ChangeToBridgeObstacle();
        }
    }

    //TODO: Account for bridge object rotation
    void ChangeToBridgeObstacle()
    {
        bridgeObstacle = true;
        obstacle.size = new Vector3(2,1,1);
        obstacle.center = new Vector3(1.5f,0,0);
    }

    public void BlockPath(bool enabled)
    {
        if(bridgeObstacle)
            obstacle.enabled = !enabled;
        obstacle.enabled = enabled;
    }
}
