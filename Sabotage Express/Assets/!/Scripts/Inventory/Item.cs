using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.Netcode;


public class Item : NetworkBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public ItemScript item;
    public Image image;
    public Transform parentAfterDrag;
    public TextMeshProUGUI amountText;
    public int amount=1;

    private void Awake(){
        RefreshAmount();
    }

    void Update()
    {
        
    }
    private ItemScript FindItemById(int itemId)
    {
        Debug.Log("Found item with NAME - " + ItemDatabase.Instance.FindItemById(itemId).itemName);
        return ItemDatabase.Instance.FindItemById(itemId);
    }
    [ClientRpc]
    public void InitializeItemClientRpc()
    {
        Debug.Log("started to initialize item");
        ItemScript newItem=FindItemById(1);
        
        Debug.Log(1+"NEW ITEM ID");
        Debug.Log(newItem.itemID);
        item=newItem;
        image.sprite=newItem.image;
        RefreshAmount();
    }
    public void RefreshAmount(){
    amountText.text=amount.ToString();
    if (amount>1){
        amountText.gameObject.SetActive(true);
    }else{
        amountText.gameObject.SetActive(false);
    }
   
}
  
     public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget=false;
        parentAfterDrag=transform.parent;
        transform.SetParent(transform.parent.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
       transform.position=Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget=true;
        transform.SetParent(parentAfterDrag);
    }




   
}
