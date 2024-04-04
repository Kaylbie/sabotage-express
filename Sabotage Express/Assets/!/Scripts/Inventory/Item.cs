using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Item : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public ItemScript item;
    public Image image;
    public Transform parentAfterDrag;
    public TextMeshProUGUI amountText;
    public int amount=1;

    private void Awake(){
        RefreshAmount();
    }

    public void InitializeItem(ItemScript newItem){
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
