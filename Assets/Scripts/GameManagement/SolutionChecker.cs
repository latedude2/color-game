using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class SolutionChecker : MonoBehaviour
{
    VisibleObjectVisibility[] visibleObjects;
    GameObject solutionCheckpoint;
    GameObject player;

    [SerializeField] GameObject[] activatableObjects;
    Activatable[] _activatables;

    bool solutionFound = false;

    private NavMeshPath path;
    // Start is called before the first frame update
    void Start()
    {
        List<Activatable> activatables = new List<Activatable>(transform.GetComponents<Activatable>());
        foreach (GameObject obj in activatableObjects)
        {
            activatables.AddRange(obj.GetComponents<Activatable>());
        }
        _activatables = activatables.ToArray();

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

    //Calls check solution with slight delay so physics/navigation system have time to update
    //TODO: replace with wait frames as this might bug out with lag
    void InvokeCheckSolution()
    {
        Invoke(nameof(CheckSolution), 0.1f);
    }

    void CheckSolution()
    {
        if(!solutionFound)
        {
            NavMesh.CalculatePath(player.transform.position, solutionCheckpoint.transform.position, NavMesh.AllAreas, path);

            if(path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Solution found!");
                SolutionFound();
                solutionFound = true;
                //Stop checking for solution after it was found once
                foreach(VisibleObjectVisibility visibleObject in visibleObjects)
                {
                    visibleObject.visibilityChanged.RemoveListener(InvokeCheckSolution);
                }
            }
            else if(path.status == NavMeshPathStatus.PathInvalid)
            {
                Debug.Log("Path invalid, trying again.");
                InvokeCheckSolution();  //Change to Invoke(InvokeCheckSolution, 0.1f); if we find this to be laggy in the future.
            }

            //For debugging
            for (int i = 0; i < path.corners.Length - 1; i++)
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.green, 2f);
        }
    }

    void SolutionFound()
    {
        if(_activatables.Length > 0)
        {
            foreach (Activatable activatable in _activatables)
            {
                activatable.Activate();
            }
        }
    }
}
