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
    public int objectQuantity;
    public Sprite objectImage;
    //private GunSpawner gunSpawner;
    void Start()
    {
        //gunSpawner = GameObject.Find("Player").GetComponent<GunSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact(GameObject player)
    {
        if (pickable)
        {
            InventoryManager inventory = player.GetComponent<InventoryManager>();
            if (inventory != null && pickable)
            {
                inventory.AddItem(this);
                
                GunSpawner gunSpawner = player.GetComponent<GunSpawner>();
                if (gunSpawner != null)
                {
                    gunSpawner.SpawnGunBasedOnName(objectName); 
                }
                else
                {
                    Debug.LogError("GunSpawner script not found on the specified GameObject.");
                }
            }
        }
        else
        {
            Debug.Log($"Interacted with {objectName}, but it's not pickable.");
        }
        
        
    }
}
