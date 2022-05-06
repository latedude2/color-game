using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour, Activatable
{
    List<Activatable> activatables;

    private void Start() {
        activatables = new List<Activatable>(transform.parent.GetComponentsInChildren<ColoredLight>());
        Debug.Log(activatables.Count);
        activatables.Remove(this);
        Debug.Log(activatables.Count);
    }
    
    public void Activate()
    {
        StartCoroutine(nameof(FlickerSequence));
    }

    public void Deactivate(){}

    IEnumerator FlickerSequence()
    {
        yield return new WaitForSeconds(1f);
        foreach(Activatable activatable in activatables)
        {
            activatable.Deactivate();
        }
        yield return new WaitForSeconds(.2f);
        foreach(Activatable activatable in activatables)
        {
            activatable.Activate();
        }
        yield return new WaitForSeconds(.5f);
        foreach(Activatable activatable in activatables)
        {
            activatable.Deactivate();
        }
        yield return new WaitForSeconds(.2f);
        foreach(Activatable activatable in activatables)
        {
            activatable.Activate();
        }
        yield return new WaitForSeconds(.5f);
        foreach(Activatable activatable in activatables)
        {
            activatable.Deactivate();
        }
    }

}
