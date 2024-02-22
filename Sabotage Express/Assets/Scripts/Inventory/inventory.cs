using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryData[] inventory;
    [System.Serializable]
    public  class InventoryData{
    public int objectID=0;
    public string objectName;
    public int objectQuantity=0;
    } 

    bool inventoryOpen = false;



    // Start is called before the first frame update
    void Start()
    {
        int inventorySize = 20;
        inventory = new InventoryData[inventorySize];
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)&& inventoryOpen==false){
            Debug.Log("Inventory Opened");
            inventoryOpen = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape)&& inventoryOpen==true)
        {
            inventoryOpen = false;
            Debug.Log("Inventory Closed");
        }
        if(Input.GetKeyDown(KeyCode.E))
        { 
            //cast ray check if object is pickable
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3))
            {
                //ignore objects if they do not have objectdata component
                if (!hit.collider.gameObject.GetComponent<ObjectData>())
                {
                    return;
                }
                if (hit.collider.gameObject.GetComponent<ObjectData>().pickable)
                {
                     Debug.Log("Ray cast hit" + hit.collider.gameObject.name);
                    AddItem(hit.collider.gameObject.GetComponent<ObjectData>());
                    Destroy(hit.collider.gameObject);
                }
            }           
           
        }
    }

    void AddItem(ObjectData obj)
    {
        
        //check if object exist already then add quantity if no add new item to inventory
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].objectID != 0)
            {
                if (inventory[i].objectID == obj.objectID)
                {
                    inventory[i].objectQuantity++;
                    Debug.Log("Item Added to Inventory");
                    return;
                }
            }
            if (inventory[i].objectID == 0)
            {
                //inventory[i] = new InventoryData();
                inventory[i].objectID = obj.objectID;
                inventory[i].objectQuantity = 1;
                inventory[i].objectName = obj.objectName;
                Debug.Log("Item Added to Inventory");
                return;
            }
        }

    }
    void RemoveItem()
    {
        Debug.Log("Item Removed");
    }

}
