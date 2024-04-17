using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkManagerCustom : NetworkBehaviour
{
    private int nextPlayerId = 1; // Start from 1
    public static NetworkManagerCustom Instance { get; private set; }
    

    private void Awake()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            "192.168.1.66", (ushort)7777, "0.0.0.0"
        );
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetNextPlayerId()
    {
        int id = nextPlayerId;
        nextPlayerId++; // Prepare next ID
        return id; // Return the current ID
    }
    
}
