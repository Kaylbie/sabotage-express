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
    
    [SerializeField] private Camera cam; 
    private NetworkVariable<int> playerId = new NetworkVariable<int>();
    
    
    public override void OnNetworkSpawn()
    {
        
        if (IsServer)
        {
            playerId.Value = NetworkManagerCustom.Instance.GetNextPlayerId();
        }

        if (IsClient)
        {
            currentHealth = maxHealth;
            StartCoroutine(WaitAndSetLayer());
            
        }
    }
    
    IEnumerator WaitAndSetLayer()
    {
        yield return new WaitUntil(() => playerId.Value != 0);

        SetLayerBasedOnPlayerId(playerId.Value);
    }

    private void SetLayerBasedOnPlayerId(int id)
    {
        string layerName = "Player" + id.ToString();
        int layer = LayerMask.NameToLayer(layerName);

        if (layer == -1)
        {
            Debug.LogError($"Layer {layerName} not found. Check if the layer is correctly defined in the Layer settings.");
            return; 
        }

        SetLayerRecursively(gameObject, layer);

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

    void Start()
    {

        //int playerCount = NetworkManagerCustom.Instance.getPlayerCount();
        //string layerName = "Player" + playerCount;
        //int layer = LayerMask.NameToLayer(layerName); // Get the layer ID
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
    }

    [ServerRpc]
    private void ServerRpc()
    {
        
    }
}
