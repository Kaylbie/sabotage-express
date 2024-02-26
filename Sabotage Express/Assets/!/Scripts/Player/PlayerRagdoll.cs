using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject rig;
    Collider[] ragDollColliders;
    private Rigidbody[] limbsRigidbodies;

    
    void Start()
    {
        rig = this.gameObject;
        GetRagdollBits();
        RagdollModeOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetRagdollBits()
    {
        // Get all colliders, then filter out the one attached to the GameObject with a CharacterController
        var allColliders = rig.GetComponentsInChildren<Collider>();
        var characterController = rig.GetComponent<CharacterController>();

        if (characterController != null)
        {
            // Exclude the CharacterController's collider from ragDollColliders
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
