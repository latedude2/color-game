using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    float followSpeed = 3.5f;       // Speed of the enemy when following the player
    float pathUpdateTime = 0.5f;    // How often the path is updated

    bool frozen = true;
    void Start()
    {
        UpdateState();
        GetComponent<VisibleObjectVisibility>().visibilityChanged.AddListener(UpdateState);
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("UpdatePath", 0f, pathUpdateTime);
    }

    void UpdatePath(){
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
    }

    void UpdateState()
    {
        if(GetComponent<VisibleObjectVisibility>().visible)
        {
            Unfreeze();
        }
        else
        {
            Freeze();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(!frozen && collision.gameObject.tag == "Player")
        {
            LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            levelManager.ReloadScene();
        }
    }

    void Unfreeze()
    {
        frozen = false;
        GetComponent<NavMeshAgent>().speed = followSpeed;
    }

    void Freeze()
    {
        frozen = true;
        GetComponent<NavMeshAgent>().speed = 0;
    }
}
