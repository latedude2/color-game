using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RailSystem : MonoBehaviour
{
    [SerializeField] List<RailNode> railNodes;
    [SerializeField] int startingNode = 0;
    [SerializeField] private int targetNodeID;
    RailNode currentNode;
    RailNode nextNode;
    [SerializeField] float speed = 5.0F;     // Movement speed in units per second.
    float startTime;    // Time when the movement started.
    float journeyLength; // Total distance between the markers.
    bool moving = false;
    [SerializeField] Transform movingPlatform;    

    void Start()
    {
        movingPlatform.position  = railNodes[startingNode].transform.position;
        currentNode = railNodes[startingNode];
        for(int i = 0; i < railNodes.Count; i++)
        {
            railNodes[i].GetComponent<RailNode>().nodeID = i;
        }
        nextNode = currentNode;
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
        foreach(RailNode railNode in railNodes)
        {
            if(railNode.nodeID != newTargetID)
            {
                railNode.Deactivate();
            }            
        }
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
        movingPlatform.position = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, fractionOfJourney);

        if(movingPlatform.position == nextNode.transform.position)
        {
            moving = false;
            currentNode = nextNode;
        }
    }

    
}
