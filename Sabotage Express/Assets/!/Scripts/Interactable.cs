using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Interactable : NetworkBehaviour
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
