using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Slot[] inventorySlots;
    public GameObject ItemPrefab;
    public GameObject mainInventoryUI;
    public int maxSlotSize=64;
    

    int selecetedSlot =0;
    private InputManager inputManager;

    void Start(){
        inputManager = GetComponent<InputManager>();
        selectSlot(selecetedSlot);
    }


        void Update()
    {
        if (inputManager.onFoot.Inventory.triggered)
        {
            ShowInventory();
            inputManager.onFoot.Movement.Disable();
            inputManager.onFoot.Look.Disable();
            inputManager.onFoot.Jump.Disable();
            inputManager.onFoot.Crouch.Disable();
            inputManager.onFoot.Sprint.Disable();
                    
        }

        if (inputManager.onFoot.Escape.triggered)
        {
            HideInventory();
            inputManager.onFoot.Movement.Enable();
            inputManager.onFoot.Look.Enable();
            inputManager.onFoot.Jump.Enable();
            inputManager.onFoot.Crouch.Enable();
            inputManager.onFoot.Sprint.Enable();
        }
    }

    public void selectSlot(int slotNo){
        inventorySlots[selecetedSlot].Deselect();
        inventorySlots[slotNo].Select();
        selecetedSlot=slotNo;
    }
    public void HideInventory(){
       
        mainInventoryUI.SetActive(false);
        
    }
    public void ShowInventory(){
        
        mainInventoryUI.SetActive(true);
      
    }
    public bool AddItem(ItemScript item){

        //Stacking
        for (int i=0; i<inventorySlots.Length;i++){
            Slot slot=inventorySlots[i];
            Item itemInSlot = slot.GetComponentInChildren<Item>();
            if (itemInSlot!=null&&itemInSlot.item==item&&itemInSlot.amount<maxSlotSize&&itemInSlot.item.stackable==true){
                itemInSlot.amount++;
                itemInSlot.RefreshAmount();
                return true;
            }
        }
        

        //Check for empty slots
        for (int i=0; i<inventorySlots.Length;i++){
            Slot slot=inventorySlots[i];
            Item itemInSlot = slot.GetComponentInChildren<Item>();
            if (itemInSlot==null){
                InsertItem(item,slot);
                return true;
            }
        }
        return false;
    }
    void InsertItem(ItemScript item,Slot slot){
        GameObject newItemGameObject = Instantiate(ItemPrefab,slot.transform);
        Item inventoryItem = newItemGameObject.GetComponent<Item>();
        inventoryItem.InitializeItem(item);
    }
}
