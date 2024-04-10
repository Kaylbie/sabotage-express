using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class InventoryManager : NetworkBehaviour
{
    // Start is called before the first frame update
    public Slot[] inventorySlots;
    public GameObject ItemPrefab;
    public GameObject mainInventoryUI;
    public int maxSlotSize=64;
    private Item currentItem;
    private ItemSpawner itemSpawner;
    int selecetedSlot =0;
    private InputManager inputManager;
    private Transform itemHolder;
    public GameObject currentHolding;
    void Start(){
        inputManager = GetComponent<InputManager>();
        selectSlot(selecetedSlot);
        itemSpawner = GetComponent<ItemSpawner>();
        itemHolder=transform.Find("Armature/root/hips/spine/chest/shoulder_R/upper_arm_R/lower_arm_R/hand_R/ItemHolder");
        itemSpawner=GetComponent<ItemSpawner>();
        Cursor.lockState = CursorLockMode.Confined;
    }



        void Update() {
            if (!IsOwner) return;
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

  
    
    public Item GetCurrentItem()
    {
        return currentItem;
    }

    public void selectSlot(int slotNo){
        
        if (itemSpawner != null && itemSpawner.spawnedItemArms != null)
        {
            Destroy(itemSpawner.spawnedItemArms);
        }
        if (currentHolding != null)
        {
            Destroy(currentHolding);
        }
        inventorySlots[selecetedSlot].Deselect();
        inventorySlots[slotNo].Select();
        selecetedSlot=slotNo;
        if (inventorySlots[selecetedSlot].GetComponentInChildren<Item>() != null)
        {
            currentItem = inventorySlots[selecetedSlot].GetComponentInChildren<Item>();
            if (currentItem.item.type == ItemScript.ItemType.Gun)
            {
                itemSpawner.SpawnGunBasedOnName(currentItem.item.itemName);
            }
            else
            {
                itemSpawner.SpawnItemBasedOnName(currentItem.item.itemName);
            }
            AddItemToHandItemHolder(currentItem.item.itemName);
        }
    }
    private GameObject LoadPrefab(string prefabPath)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        return prefab;
    }
    public void AddItemToHandItemHolder(String name){
        string prefabPath = "Prefabs/Items/"+name;
        GameObject prefab = LoadPrefab(prefabPath);
        if (prefab != null)
        {
            currentHolding = Instantiate(prefab, itemHolder.position, itemHolder.rotation, itemHolder);
            Rigidbody rb = currentHolding.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            Transform handPosTransform = currentHolding.transform.Find("HandPos");
            if (handPosTransform != null)
            {
                // Calculate how much we need to move the instantiated item so that HandPos aligns with the ItemHolder's position.
                // This assumes HandPos's local position effectively represents the offset within the item that should match the ItemHolder's position.
                Vector3 positionOffset = itemHolder.position - handPosTransform.position;
                currentHolding.transform.position += positionOffset;

                // No adjustments to rotation here since it's set correctly initially.
            }
            int layerIndex = gameObject.layer;
            
            SetLayerRecursively(currentHolding, layerIndex);
        }
        else
        {
            Debug.LogError("Not found " + name);
        }
    }
    private void SetLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj) {
            return;
        }

        // Set the layer of the current object
        obj.layer = newLayer;

        // Recursively set the layer of all children
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
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
                selectSlot(selecetedSlot);
                return true;
            }
        }
        return false;
    }
    void InsertItem(ItemScript item,Slot slot){
        GameObject newItemGameObject = Instantiate(ItemPrefab,slot.transform);
        Item inventoryItem = newItemGameObject.GetComponent<Item>();
        if(inventoryItem!=null){
            inventoryItem.InitializeItem(item);

        }
        else
        {
            Debug.LogError("Fix inventory item prefab");
            
        }
    }
}
