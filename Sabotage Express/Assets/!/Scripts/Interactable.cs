using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promtMessage;
    public void BaseInteract(GameObject player)
    {
        Interact(player);
    }
    protected virtual void Interact(GameObject player)
    {

    }
}
