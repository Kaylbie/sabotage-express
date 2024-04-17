using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : NetworkBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    [ServerRpc]
    public void TakeDamageServerRpc(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage.");

        if (currentHealth <= 0)
        {
            DieServerRpc();
        }
    }
    [ServerRpc]
    void DieServerRpc()
    {
        Debug.Log($"{gameObject.name} died.");
        gameObject.SetActive(false); 
        
    }
}
