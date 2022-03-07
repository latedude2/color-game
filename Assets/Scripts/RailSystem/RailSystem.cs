using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSystem : MonoBehaviour
{
    [SerializeField] List<RailNode> railNodes;
    [SerializeField] int startingNode = 0;

    [SerializeField] private int targetNodeID; // field

    RailNode currentNode;
    RailNode nextNode;


    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;
    
    bool moving = false;

    [SerializeField] MovingPlatform movingPlatform;

    void Start()
    {
        movingPlatform.transform.position  = railNodes[startingNode].transform.position;
        currentNode = railNodes[startingNode];
        for(int i = 0; i < railNodes.Count; i++)
        {
            railNodes[i].GetComponent<RailNode>().nodeID = i;
        }
    }

    void Update() {
        if(!moving)
        {
            RailNode unchangedNextNode = nextNode;
            nextNode = railNodes[FindNextNodeID()].GetComponent<RailNode>();
            if(unchangedNextNode != nextNode) //if the next node has changed
            {
                moving = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(currentNode.transform.position, nextNode.transform.position);
            }
        }
        else
            MoveToNextNode();
    }

    public void SetTargetID(int newTargetID)
    {
        targetNodeID = newTargetID;
    }

    int FindNextNodeID()
    {
        if (currentNode.nodeID < targetNodeID)
            return currentNode.nodeID + 1;
        else if (currentNode.nodeID > targetNodeID)
            return currentNode.nodeID - 1;
        else
            return targetNodeID;
    }

    void MoveToNextNode()
    {
        float distCovered = (Time.time - startTime) * speed;
        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;
        // Set our position as a fraction of the distance between the markers.
        movingPlatform.transform.position = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, fractionOfJourney);

        if(movingPlatform.transform.position == nextNode.transform.position)
        {
            moving = false;
            currentNode = nextNode;
        }
    }

    
}
