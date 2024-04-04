using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    private GameObject currentItem;
    private GameObject modelPrefab;
    private string prefabNamePattern = "Assault_Rifle_0";
    public int currentPrefabNumber = 1;
    private InputManager inputManager;
    private string gunName;
    private GunSpawner gunSpawner;
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        gunSpawner = GetComponent<GunSpawner>();
    }

    void Update()
    {
        if (inputManager.onFoot.DropItem.triggered)
        {
            DropItem();
        }
    }
    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return prefab;
    }

    public void DropItem()
    {
        if (currentItem != null)
        {
            // Get the position and rotation of the current item
            Vector3 itemPosition = currentItem.transform.position;
            Quaternion itemRotation = currentItem.transform.rotation;
            if (gunSpawner != null)
            {
                gunName = gunSpawner.gunName;
                //Debug.Log("Gun Name: " + gunName);
            }
            else
            {
                Debug.LogError("GunSpawner component not found");
            }
            // Destroy the currently held item
            Destroy(currentItem);

            // Find the model prefab based on the naming pattern
            string prefabPath = "Assets/!/Prefabs/Models_Only/Guns/"+gunName+".prefab";
    
            GameObject prefab = LoadPrefab(prefabPath);
    
            if (prefab != null)
            {
                //Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
                Instantiate(prefab, itemPosition, itemRotation);
            }
            else
            {
                Debug.LogError("Failed to load prefab at path: " + prefabPath);
            }
        }
        else
        {
            Debug.LogWarning("No item currently held to drop.");
        }
    }

    public void PickUpItem(GameObject item)
    {
        currentItem = item;
    }
    private string FindPrefabToSpawn()
    {
        string prefabPath = "Assets/!/Prefabs/Models_Only/Guns/Assault_Rifle_01.prefab"; // Full path of the prefab
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab != null)
        {
            Debug.Log("Prefab found: " + prefabPath);
            return prefabPath;
        }
        else
        {
            Debug.Log("Prefab not found: " + prefabPath);
            return null;
        }
    }
}
