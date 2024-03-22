using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public MatchEntity ownerMatchEntity;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovablePair ColliderMovable))
        {
            ownerMatchEntity.PairObjectInteraction(true, ColliderMovable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MovablePair ColliderMovable))
        {
            ownerMatchEntity.PairObjectInteraction(false, ColliderMovable);
        }
    }
}
