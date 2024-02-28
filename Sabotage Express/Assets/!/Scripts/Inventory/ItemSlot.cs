using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
//Item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;
    // Start is called before the first frame update    
    public void AddItem(Item item){
        itemName = item.objectName;
        quantity = item.objectQuantity;
        itemSprite = item.objectImage;
        itemImage.sprite = itemSprite;
        
        quantityText.text = quantity.ToString();
        
        itemImage.enabled = true;
        isFull = true;
    }
}
