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

    private Transform chest;
    private Transform neck;
    private Transform head;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; } 
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

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        if (prefabToRotate != null)
        {
            prefabToRotate.rotation = cam.transform.rotation;
        }
        

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        if (spine != null)
        {
            float tiltFactor = 0.3f; 
            Quaternion tiltRotation = Quaternion.Euler(xRotation * tiltFactor, 0, 0);

            spine.transform.localRotation = tiltRotation;
            if (chest != null) chest.localRotation = tiltRotation;
            if (neck != null) neck.localRotation = tiltRotation;
            if (head != null) head.localRotation = tiltRotation;
        }
    }
}
