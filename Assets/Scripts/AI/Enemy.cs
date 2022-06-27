using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    float followSpeed = 3.5f;
    void Start()
    {
        UpdateSpeed();
        GetComponent<VisibleObjectVisibility>().visibilityChanged.AddListener(UpdateSpeed);
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath(){
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
    }

    void UpdateSpeed()
    {
        if(GetComponent<VisibleObjectVisibility>().visible)
        {
            GetComponent<NavMeshAgent>().speed = followSpeed;
        }
        else
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }
    }

    private void FixedUpdate() {
        
    }

    //On collision with player, damage player
    private void OnCollisionEnter(Collision collision) {
        if(GetComponent<VisibleObjectVisibility>().visible && collision.gameObject.tag == "Player")
        {
            LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            levelManager.ReloadScene();
        }
    }
}
