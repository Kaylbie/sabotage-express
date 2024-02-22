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

    protected override void Interact()
    {
        if (pickable)
        {
            Inventory.Instance.AddItemToInventory(this);
            // Since the object is added to the inventory, you might want to disable it instead of destroying it immediately.
            // This allows for the possibility of dropping the item back into the world.
            // Object.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"Interacted with {objectName}, but it's not pickable.");
        }
        
        
    }
}
