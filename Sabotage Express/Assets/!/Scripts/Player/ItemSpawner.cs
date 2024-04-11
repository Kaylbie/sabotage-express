using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{

    //private Camera cam;
    //public GameObject gunPrefab1;
    //public GameObject gunPrefab2;
    //public GameObject gunPrefab3;
    private PlayerMotor playerMotor;
    private PlayerLook playerLook;
    //private Dictionary<int, GameObject> gunPrefabs = new Dictionary<int, GameObject>();
    public string gunName;
    public Transform bulletSpawnPoint;
    public Transform handTransform;
    public GameObject spawnedItemArms;
    private GameObject currentHolding;
    private Transform itemHolder;

    private string itemName;
    private void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMotor = GetComponent<PlayerMotor>();
        
    }

    private void ChangeSpawnLocation()
    {
        bulletSpawnPoint.transform.position = playerLook.cam.transform.position;
        bulletSpawnPoint.transform.rotation = playerLook.cam.transform.rotation;
    }
    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        return prefab;
    }
    public void SpawnGunBasedOnName(string name)
    {
        gunName = name;
        string prefabPath = "Prefabs/ARM PREFABS feature/"+gunName+"_Arms";
        GameObject prefab = LoadPrefab(prefabPath);
        if (prefab != null)
        {
            Vector3 playerCoordinates = gameObject.transform.position;
            spawnedItemArms = Instantiate(prefab, playerCoordinates, Quaternion.identity);
            spawnedItemArms.transform.localScale = Vector3.one;
            
            playerMotor.SetArmsTransform(spawnedItemArms.transform);
            spawnedItemArms.transform.parent = gameObject.transform;
            playerLook.prefabToRotate = spawnedItemArms.transform;
            spawnedItemArms.transform.localScale = Vector3.one;
            //ItemDropper itemDropper = player.GetComponent<ItemDropper>();
            //itemDropper.PickUpItem(spawnedGun);
        }
        else
        {
            Debug.LogError("Gun name not found: " + name);
        }
    }
    public void SpawnItemBasedOnName(string name)
    {
        string prefabPath = "Prefabs/ARM PREFABS feature/Item_Arms";
        GameObject prefab = LoadPrefab(prefabPath);
        if (prefab != null)
        {
            Vector3 playerCoordinates = gameObject.transform.position;
            //Vector3 cameraCoordinates = playerLook.cam.transform.localPosition;
            spawnedItemArms = Instantiate(prefab, playerCoordinates, Quaternion.identity);
            spawnedItemArms.transform.localScale = Vector3.one;
            
            playerMotor.SetArmsTransform(spawnedItemArms.transform);
            spawnedItemArms.transform.parent = gameObject.transform;
            playerLook.prefabToRotate = spawnedItemArms.transform;
            spawnedItemArms.transform.localScale = Vector3.one;
            
            string prefabPath2 = "Prefabs/Items/"+name;
            GameObject prefab2 = LoadPrefab(prefabPath2);
            if (prefab2 != null)
            {
                itemHolder=spawnedItemArms.transform.Find("arms_item/Armature/arm_R/lower_arm_R/hand_R/ItemHolder");

                currentHolding = Instantiate(prefab2, itemHolder.position, itemHolder.rotation, itemHolder);
                Rigidbody rb = currentHolding.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                Transform handPosTransform = currentHolding.transform.Find("HandPos");
                     
                     if (itemHolder != null)
                     {
                         if (handPosTransform != null)
                         {
                             Vector3 positionOffset = itemHolder.position - handPosTransform.position;
                             currentHolding.transform.position += positionOffset;
                
                         }
                         int invisibleLayer = LayerMask.NameToLayer("GunCam");
                         SetLayerRecursively(currentHolding, invisibleLayer);
                  }
                     
            }
            else
            {
                Debug.LogError("Not found " + name);
            }
            
            
        }
        else
        {
            Debug.LogError("Item name not found: " + name);
        }
    }
    private void SetLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj) {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        ChangeSpawnLocation();
    }
}
