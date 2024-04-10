using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class ItemDropper : NetworkBehaviour
{
    private InventoryManager inventoryManager;
    private InputManager inputManager;
    private ItemSpawner itemSpawner;
    public float throwForce = 500f; 

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        inventoryManager=GetComponent<InventoryManager>();
        itemSpawner = GetComponent<ItemSpawner>();
    }
    void Update()
    {
        if (!IsOwner) return;
        if (inputManager.onFoot.DropItem.triggered)
        {
            DropItem();
        }
    
        //currentItem = inventoryManager.GetCurrentItem();
    }

    private void DropItem()
    {
        if (inventoryManager.currentHolding != null)
        {
            inventoryManager.currentHolding.transform.SetParent(null);
            Rigidbody rb = inventoryManager.currentHolding.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            if (itemSpawner != null && itemSpawner.spawnedItemArms != null)
            {
                Destroy(itemSpawner.spawnedItemArms);
            }
            rb.AddForce(inventoryManager.currentHolding.transform.forward * throwForce);
            int invisibleLayer = LayerMask.NameToLayer("Interactable");
            SetLayerRecursively(inventoryManager.currentHolding, invisibleLayer);
            inventoryManager.currentHolding = null;
        }
        else
        {
            Debug.Log("No item to drop");
        }
        
    }
    private void SetLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj) {
            return;
        }

        // Set the layer of the current object
        obj.layer = newLayer;

        // Recursively set the layer of all children
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    // private GameObject modelPrefab;
    // private InputManager inputManager;
    // private string gunName;
    // private GunSpawner gunSpawner;
    // private InventoryManager inventoryManager;
    // private Item currentItem;
    // private GameObject currentItemObject;
    // void Start()
    // {
    //     inputManager = GetComponent<InputManager>();
    //     gunSpawner = GetComponent<GunSpawner>();
    //     inventoryManager=GetComponent<InventoryManager>();
    // }
    //
    // void Update()
    // {
    //     if (inputManager.onFoot.DropItem.triggered)
    //     {
    //         DropItem();
    //     }
    //
    //     currentItem = inventoryManager.GetCurrentItem();
    // }
    // private GameObject LoadPrefab(string prefabPath)
    // {
    //     GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    //     return prefab;
    // }
    //
    // public void DropItem()
    // {
    //     if (currentItem != null)
    //     {
    //         // Get the position and rotation of the current item
    //         Vector3 itemPosition = currentItem.transform.position;
    //         Quaternion itemRotation = currentItem.transform.rotation;
    //         if (gunSpawner != null)
    //         {
    //             gunName = gunSpawner.gunName;
    //             //Debug.Log("Gun Name: " + gunName);
    //         }
    //         else
    //         {
    //             Debug.LogError("GunSpawner component not found");
    //         }
    //         // Destroy the currently held item
    //         Destroy(currentItem);
    //
    //         // Find the model prefab based on the naming pattern
    //         string prefabPath = "Assets/!/Prefabs/Models_Only/Guns/"+gunName+".prefab";
    //
    //         GameObject prefab = LoadPrefab(prefabPath);
    //
    //         if (prefab != null)
    //         {
    //             //Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
    //             Instantiate(prefab, itemPosition, itemRotation);
    //         }
    //         else
    //         {
    //             Debug.LogError("Failed to load prefab at path: " + prefabPath);
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogWarning("No item currently held to drop.");
    //     }
    // }
    //
    // public void PickUpItem(GameObject item)
    // {
    //     currentItem = item;
    // }
    // private string FindPrefabToSpawn()
    // {
    //     string prefabPath = "Assets/!/Prefabs/Models_Only/Guns/Assault_Rifle_01.prefab"; // Full path of the prefab
    //     GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    //
    //     if (prefab != null)
    //     {
    //         Debug.Log("Prefab found: " + prefabPath);
    //         return prefabPath;
    //     }
    //     else
    //     {
    //         Debug.Log("Prefab not found: " + prefabPath);
    //         return null;
    //     }
    // }
}
