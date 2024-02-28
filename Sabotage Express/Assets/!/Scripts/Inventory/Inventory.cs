using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    private List<Item> inventoryItems;
    
    bool inventoryOpen = false;
    public GameObject inventoryUI;
    
    // public void AddItemToInventory(Item item)
    // {
    //     inventoryItems.Add(item);
    //     Debug.Log($"{item} added to {this.gameObject.GetComponent<Player>().nickname} inventory");
    //     //Destroy(item.gameObject);


    // }
    // // Start is called before the first frame update
    // void Start()
    // {
        
    //     inventoryUI.SetActive(false);
    //     int inventorySize = 20;
    //     inventoryItems = new List<Item>(inventorySize);
    // }
    // // Update is called once per frame
    
    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Escape)&& inventoryOpen==true||Input.GetKeyDown(KeyCode.I)&& inventoryOpen==true)
    //     {
    //         inventoryOpen = false;
    //         Debug.Log("Inventory Closed");
    //         inventoryUI.SetActive(false);
    //     }
    //     if(Input.GetKeyDown(KeyCode.I)&& inventoryOpen==false){
    //         Debug.Log("Inventory Opened");
    //         inventoryOpen = true;
    //         inventoryUI.SetActive(true);
    //         foreach (Item item in inventoryItems)
    //         {
    //             Debug.Log(item.objectName);
    //         }
    //     }

    // }

    // void RemoveItem()
    // {
    //     Debug.Log("Item Removed");
    // }

}
