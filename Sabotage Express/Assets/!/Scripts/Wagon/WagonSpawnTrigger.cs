using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonSpawnTrigger : MonoBehaviour
{
    public TrainWagonGen trainWagonGenScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trainWagonGenScript.SpawnWagon();
        }
    }
}
