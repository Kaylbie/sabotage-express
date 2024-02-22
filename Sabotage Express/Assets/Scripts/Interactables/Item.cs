using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public string objectName;
    public string objectDescription;
    public int objectID;
    public int objectWeight;
    public int objectValue;
    public bool pickable;
    public int objectQuantity = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player)
    {
        if (pickable)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null && pickable)
            {
                inventory.AddItemToInventory(this);
                
            }
        }
        else
        {
            Debug.Log($"Interacted with {objectName}, but it's not pickable.");
        }
        
        
    }
}
