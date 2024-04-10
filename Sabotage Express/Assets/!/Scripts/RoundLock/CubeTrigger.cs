using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    public RoundLock roundLock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CylindarTag"))
        {
            roundLock.endGame();
        }
    }
}
