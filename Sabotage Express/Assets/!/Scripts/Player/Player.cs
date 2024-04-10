using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public string nickname;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject healthBar;
    public Transform enemy;
    [SerializeField] private Camera cam; // Assign in the Inspector
    private NetworkVariable<int> playerId = new NetworkVariable<int>();
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Only the server assigns player IDs
            playerId.Value = NetworkManagerCustom.Instance.GetNextPlayerId();
        }

        if (IsClient)
        {
            StartCoroutine(WaitAndSetLayer());
        }
    }

    IEnumerator WaitAndSetLayer()
    {
        // Wait until the playerId has been synchronized to this client
        yield return new WaitUntil(() => playerId.Value != 0);

        SetLayerBasedOnPlayerId(playerId.Value);
    }

    private void SetLayerBasedOnPlayerId(int id)
    {
        // Construct the layer name based on the player ID
        string layerName = "Player" + id.ToString();
        int layer = LayerMask.NameToLayer(layerName);

        if (layer == -1)
        {
            Debug.LogError($"Layer {layerName} not found. Check if the layer is correctly defined in the Layer settings.");
            return; // Exit if the layer was not found
        }

        // Set the layer for this GameObject and all its children
        SetLayerRecursively(gameObject, layer);

        // Adjust the camera's culling mask to ignore this player's layer
        if (cam != null)
        {
            cam.cullingMask &= ~(1 << layer);
        }
        else
        {
            Debug.LogError("Camera not found.");
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //int playerCount = NetworkManagerCustom.Instance.getPlayerCount();
        //string layerName = "Player" + playerCount;
        
        cam = gameObject.GetComponent<PlayerLook>().cam;
        // Debug.Log(gameObject);
        // if (cam != null)
        // {
        //     SetLayer();
        //     Debug.Log("cam changed");   
        // }
    }
    // private void SetLayer()
    // {
    //     string layerName = "Player" + NetworkManagerCustom.Instance.getPlayerCount();
    //     Debug.Log(NetworkManagerCustom.Instance.getPlayerCount()+" count");
    //     int layer = LayerMask.NameToLayer(layerName);
    //     SetLayerRecursively(gameObject, layer);
    //     int layerMask = 1 << layer;
    //     cam.cullingMask &= ~layerMask;
    //     NetworkManagerCustom.Instance.addCount();
    // }
    
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
        Transform newEnemy = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
        if (newEnemy != null)
        {
            NetworkObject netObj = newEnemy.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                netObj.Spawn();
                Debug.Log("Enemy spawned");
            }
            else
            {
                Debug.LogError("Spawn failed: NetworkObject component not found on the instantiated enemy prefab.");
            }
        }
        else
        {
            Debug.LogError("Spawn failed: Instantiation of the enemy prefab returned null.");
        }

    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);
        ChangeHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    string GenerateRandomNickname(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        System.Random random = new System.Random();
        char[] stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }


      public void ChangeHealthBar(){
        healthBar.GetComponent<UnityEngine.UI.Slider>().value=currentHealth;
    }

    void Die()
    {
        Debug.Log("Player dies!");
        // Here, handle the player's death (e.g., show a retry screen)
    }

    [ServerRpc]
    private void ServerRpc()
    {
        
    }
}
