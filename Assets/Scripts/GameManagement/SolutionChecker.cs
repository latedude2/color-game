using UnityEngine;
using UnityEngine.AI;

public class SolutionChecker : MonoBehaviour
{
    VisibleObjectVisibility[] visibleObjects;
    GameObject solutionCheckpoint;
    GameObject player;

    private NavMeshPath path;
    // Start is called before the first frame update
    void Start()
    {
        visibleObjects = GameObject.FindObjectsOfType<VisibleObjectVisibility>();
        
        solutionCheckpoint = GameObject.Find("SolutionCheckPoint");
        if(solutionCheckpoint == null)
        {
            Debug.LogWarning("Solution checker did not find a solution check point! Must have transform in scene named \"SolutionCheckPoint\" which denotes the end of the level for solution checking to work.", transform);
            return;
        }
        
        player = GameObject.Find("Hero_Prefab");
        if(player == null)
        {
            Debug.LogWarning("Solution checker did not find player! Must have transform in scene named \"Hero_Prefab\" which is attached to player character.", transform);
            return;
        }

        foreach(VisibleObjectVisibility visibleObject in visibleObjects)
        {
            visibleObject.visibilityChanged.AddListener(InvokeCheckSolution);
        }
        path = new NavMeshPath();
    }

    //Calls check solution with slight delay so physics have time to update
    void InvokeCheckSolution()
    {
        Invoke(nameof(CheckSolution), 0.1f);
    }

    void CheckSolution()
    {
        NavMesh.CalculatePath(player.transform.position, solutionCheckpoint.transform.position, NavMesh.AllAreas, path);

        //TODO: retry if path invalid
        if(path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.Log("Solution found!");
            //Stop checking for solution after it was found once
            foreach(VisibleObjectVisibility visibleObject in visibleObjects)
            {
                visibleObject.visibilityChanged.RemoveListener(InvokeCheckSolution);
            }
        }
        else if(path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Path invalid, trying again.");
            InvokeCheckSolution();
        }

        //For debugging
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 2f);
    }

    

    
}
