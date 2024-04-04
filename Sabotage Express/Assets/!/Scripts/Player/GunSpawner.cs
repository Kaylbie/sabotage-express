using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject player;

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
    public GameObject spawnedGun;
    private void Start()
    {
        //gunPrefabs.Add(1, gunPrefab1);
        //gunPrefabs.Add(2, gunPrefab2);
        //gunPrefabs.Add(3, gunPrefab3);
        playerLook = GetComponent<PlayerLook>();
        playerMotor = player.GetComponent<PlayerMotor>();
        
    }

    private void ChangeSpawnLocation()
    {
        bulletSpawnPoint.transform.position = playerLook.cam.transform.position;
        bulletSpawnPoint.transform.rotation = playerLook.cam.transform.rotation;
    }
    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        return prefab;
    }
    public void SpawnGunBasedOnName(string name)
    {
        gunName = name;
        string prefabPath = "Assets/!/Prefabs/ARM PREFABS feature/"+gunName+"_Arms.prefab";
        GameObject prefab = LoadPrefab(prefabPath);
        if (prefab != null)
        {
            Vector3 playerCoordinates = player.transform.position;
            //Vector3 cameraCoordinates = playerLook.cam.transform.localPosition;
            spawnedGun = Instantiate(prefab, playerCoordinates, Quaternion.identity);
            spawnedGun.transform.localScale = Vector3.one;
            
            playerMotor.SetArmsTransform(spawnedGun.transform);
            spawnedGun.transform.parent = handTransform;
            playerLook.prefabToRotate = spawnedGun.transform;
            spawnedGun.transform.localScale = Vector3.one;
            //ItemDropper itemDropper = player.GetComponent<ItemDropper>();
            //itemDropper.PickUpItem(spawnedGun);
        }
        else
        {
            Debug.LogError("Gun name not found: " + name);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        ChangeSpawnLocation();
    }
}
