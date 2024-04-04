using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProp : Interactable
{
    // Start is called before the first frame update
    public ItemScript itemScript;

        protected override void Interact(GameObject player)
    {
        Debug.Log($"Interacted with {itemScript.itemName}");
        {
            InventoryManager inventory = player.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(itemScript);
                Destroy(this.gameObject);
            }
        }
    }
}
