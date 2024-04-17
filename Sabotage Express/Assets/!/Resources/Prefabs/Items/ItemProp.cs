using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemProp : Interactable
{
    // Start is called before the first frame update
    public ItemScript itemScript;

    protected override void Interact(GameObject player)
    {
        //if (!IsOwner) return;
        Debug.Log($"Interacted with {itemScript.itemName}");
        InventoryManager inventory = player.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(itemScript.itemID);
            
            DestroyItemServerRpc();
        }
        else
        {
            Debug.LogError("Inventory null");
            
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyItemServerRpc()
    {
        Destroy(this.gameObject);
    }
}
