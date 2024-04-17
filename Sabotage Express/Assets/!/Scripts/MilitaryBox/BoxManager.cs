using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BoxManager : NetworkBehaviour
{
    [SerializeField] private RoundLock roundLock;
    [SerializeField] private RotatePanel rotatePanel;

    [SerializeField] private bool unlocked = false;

    [SerializeField] private GameObject spanwpoint;
    [SerializeField] private GameObject objectToSpawn;

    void Start()
    {
        SpawnObjectServerRpc();
    }

    void Update()
    {
        if (!unlocked)
        {
            unlocked = roundLock.IsAccessGranted();

        }
        rotatePanel.enabled = unlocked;
    }

    [ServerRpc]
    private void SpawnObjectServerRpc()
    {
        GameObject obj = Instantiate(objectToSpawn, spanwpoint.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
    }
}
