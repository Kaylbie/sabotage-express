using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetection : MonoBehaviour
{
    public BlockSpawner blockSpawner;

    private void OnTriggerEnter(Collider other)
    {
        blockSpawner.DestroyBlock(other.gameObject);
    }
}
