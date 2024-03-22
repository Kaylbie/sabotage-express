using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private RotatePanel rotatePanel;

    [SerializeField] private bool unlocked = false;

    [SerializeField] private GameObject spanwpoint;
    [SerializeField] private GameObject objectToSpawn;

    void Start()
    {
        GameObject obj = Instantiate(objectToSpawn, spanwpoint.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (!unlocked)
        {
            unlocked = blockSpawner.IsAccessGranted();

        }
        rotatePanel.enabled = unlocked;
    }
}
