using UnityEngine;
//This interface is used for objects that activate other objects to ensure the reference to the activatable object is removed once the activatable object is deleted
public interface Activater //Activator was taken :(
{
    void RemoveActivatable(GameObject activatable);
}
