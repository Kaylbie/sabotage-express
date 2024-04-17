using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private GameObject rig;
    Collider[] ragDollColliders;
    private Rigidbody[] limbsRigidbodies;

    
    void Start()
    {
        rig = this.gameObject;
        GetRagdollBits();
        RagdollModeOff();
    }

    void Update()
    {
        
    }
    void GetRagdollBits()
    {
        var allColliders = rig.GetComponentsInChildren<Collider>();
        var characterController = rig.GetComponent<CharacterController>();

        if (characterController != null)
        {
            ragDollColliders = allColliders.Where(col => col != characterController).ToArray();
        }
        else
        {
            ragDollColliders = allColliders;
        }

        limbsRigidbodies = rig.GetComponentsInChildren<Rigidbody>();
    }
    
    void RagdollModeOn()
    {
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in limbsRigidbodies)
        {
            rb.isKinematic = false;
        }
    }

    void RagdollModeOff()
    {
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in limbsRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
}
