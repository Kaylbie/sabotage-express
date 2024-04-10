using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerLook : NetworkBehaviour
{
    public Camera cam;
    public Transform prefabToRotate;
    public GameObject spine;
    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    private string playerLayerName;

    // Reference to child transforms
    private Transform chest;
    private Transform neck;
    private Transform head;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; } // ALL players will read this method, only player owner will execute past this line
        base.OnNetworkSpawn();
        cam.enabled = true;
        Debug.Log("cam on");
    }
    
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
   
        obj.layer = newLayer;
   
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void Start()
    {
        
        // Assuming the names of the child objects are exactly "spine", "chest", "neck", "head"
        // Find them as children of the spine GameObject
        chest = spine.transform.Find("chest");
        neck = chest != null ? chest.Find("neck") : null;
        head = neck != null ? neck.Find("head") : null;
    }

    void Update()
    {
        //if (!IsOwner) return;
    }
    public void setPrafabRotate(Transform prefabRotate)
    {
        this.prefabToRotate = prefabRotate;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // Adjust the rotation based on input
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply rotation to camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        if (prefabToRotate != null)
        {
            prefabToRotate.rotation = cam.transform.rotation;
        }
        

        // Rotate the player body
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        // Apply a fraction of this rotation to the spine, chest, neck, and head
        // This fraction can be adjusted to suit the desired effect
        if (spine != null)
        {
            float tiltFactor = 0.3f; // Adjust this to get a more or less pronounced tilt
            Quaternion tiltRotation = Quaternion.Euler(xRotation * tiltFactor, 0, 0);

            // Apply the rotation. You can adjust each body part's rotation factor individually if needed
            spine.transform.localRotation = tiltRotation;
            if (chest != null) chest.localRotation = tiltRotation;
            if (neck != null) neck.localRotation = tiltRotation;
            if (head != null) head.localRotation = tiltRotation;
        }
    }
}
