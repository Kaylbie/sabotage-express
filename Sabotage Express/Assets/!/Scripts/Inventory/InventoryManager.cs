using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryUI;
    public bool inventoryOpen = false;
    public ItemSlot[] itemSlot;
    public int selectedSlot=0;
    private InputManager inputManager;
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        selectSlot(selectedSlot);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.onFoot.Inventory.triggered)
        {
            openInventory();
            inputManager.onFoot.Movement.Disable();
            inputManager.onFoot.Look.Disable();
            inputManager.onFoot.Jump.Disable();
            inputManager.onFoot.Crouch.Disable();
            inputManager.onFoot.Sprint.Disable();
                    
        }

        if (inputManager.onFoot.Escape.triggered)
        {
            closeInventory();
            inputManager.onFoot.Movement.Enable();
            inputManager.onFoot.Look.Enable();
            inputManager.onFoot.Jump.Enable();
            inputManager.onFoot.Crouch.Enable();
            inputManager.onFoot.Sprint.Enable();
        }
        if(inputManager.onFoot.hotbar1.triggered)
        {
            selectSlot(1);
        }
        if(inputManager.onFoot.hotbar2.triggered)
        {
            selectSlot(2);
        }
        if(inputManager.onFoot.hotbar3.triggered)
        {
            selectSlot(3);
        }
        if(inputManager.onFoot.hotbar4.triggered)
        {
            selectSlot(4);
        }
        if(inputManager.onFoot.hotbar5.triggered)
        {
            selectSlot(5);
        }
        if(inputManager.onFoot.hotbar6.triggered)
        {
            selectSlot(6);
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

    public void openInventory()
    {
        Debug.Log("Inventory Opened");
        Cursor.visible = true;
        inventoryOpen = true;
        inventoryUI.SetActive(true);
    }
    public void closeInventory()
    {
        inventoryOpen = false;
        Debug.Log("Inventory Closed");
        Cursor.visible = false;
        inventoryUI.SetActive(false);
    }

    public void selectSlot(int slot)
    {
        itemSlot[selectedSlot].UnmarkSelected();
        selectedSlot = slot-1;
        itemSlot[selectedSlot].MarkSelected();
        
    }

}
