using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPoints : NetworkBehaviour
{
    public List<Transform> spawnPoints;
    public Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemyServerRpc();
            
        }
    }
    [ServerRpc]
    private void SpawnEnemyServerRpc()
    {
        if (spawnPoints.Count < 2)
        {
            Debug.LogError("Not enough spawn points specified.");
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;
            Transform newEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);

            NetworkObject netObj = newEnemy.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                netObj.Spawn();
                Debug.Log($"Enemy spawned at spawn point {i}");
            }
            else
            {
                Debug.LogError("Spawn failed: NetworkObject component not found on the instantiated enemy prefab.");
            }
        }
    }
}
