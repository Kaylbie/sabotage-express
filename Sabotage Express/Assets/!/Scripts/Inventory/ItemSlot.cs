using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour,IDragHandler, IEndDragHandler, IBeginDragHandler
{

    public Transform parentAfterDrag;

    //TO DO 
    // REMOVE ITEMS MAKE SO WHEN ITS BEING ADDED IT CREATES ITEM PREFAB AND PLACES IT UNDER ITEMSLOT
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent);
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        itemImage.raycastTarget = true;
    }
//Item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public GameObject selectedSprite;
    public bool isFull;

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;
    // Start is called before the first frame update    
    public void AddItem(Item item){
        itemName = item.itemName;
        quantity = item.quantity;
        itemSprite = item.itemImage;
        itemImage.sprite = itemSprite;
        
        quantityText.text = quantity.ToString();
        
        itemImage.enabled = true;
        isFull = true;
    }

    public void MarkSelected(){
         selectedSprite.SetActive(true);
    }
    public void UnmarkSelected(){
        selectedSprite.SetActive(false);
    }
}
