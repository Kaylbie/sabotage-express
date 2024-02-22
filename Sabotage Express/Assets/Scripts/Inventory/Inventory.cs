using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private List<Item> inventoryItems = new List<Item>();
    
    public InventoryData[] inventory;
    [System.Serializable]
    public  class InventoryData{
    public int objectID=0;
    public string objectName;
    public int objectQuantity=0;
    } 

    bool inventoryOpen = false;
    public GameObject inventoryUI;

    private void Awake()
    {
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
    public void AddItemToInventory(Item item)
    {
        inventoryItems.Add(item);
        // Optionally, update UI or notify the player here
        Debug.Log($"Added {item.objectName} to inventory");
    }
    // Start is called before the first frame update
    void Start()
    {
        
        inventoryUI.SetActive(false);
        int inventorySize = 20;
        inventory = new InventoryData[inventorySize];
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)&& inventoryOpen==false){
            Debug.Log("Inventory Opened");
            inventoryOpen = true;
            inventoryUI.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Escape)&& inventoryOpen==true)
        {
            inventoryOpen = false;
            Debug.Log("Inventory Closed");
            inventoryUI.SetActive(false);
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

    public void AddItem(ObjectData obj)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].objectID != 0)
            {
                if (inventory[i].objectID == obj.objectID)
                {
                    inventory[i].objectQuantity= inventory[i].objectQuantity + obj.objectQuantity;
                    Debug.Log("Item Added to Inventory");
                    return;
                }
            }
            if (inventory[i].objectID == 0)
            {
                //inventory[i] = new InventoryData();
                inventory[i].objectID = obj.objectID;
                inventory[i].objectQuantity = obj.objectQuantity;
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
