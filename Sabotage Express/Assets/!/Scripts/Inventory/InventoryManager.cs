using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryUI;
    public bool inventoryOpen = false;
    public ItemSlot[] itemSlot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&& inventoryOpen==true||Input.GetKeyDown(KeyCode.I)&& inventoryOpen==true)
        {
            inventoryOpen = false;
            Debug.Log("Inventory Closed");
            inventoryUI.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.I)&& inventoryOpen==false){
            Debug.Log("Inventory Opened");
            inventoryOpen = true;
            inventoryUI.SetActive(true);
        }
    }
    public void AddItem(Item item)
    {
        Debug.Log($"{item} added to {this.gameObject.GetComponent<Player>().nickname} inventory");
        foreach (ItemSlot slot in itemSlot)
        {
            if (!slot.isFull)
            {
                slot.AddItem(item);
                Destroy(item.gameObject);
                return;
            }
        }
        
        Debug.Log(item.objectImage);
    }
}
