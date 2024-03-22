using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
      Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
      GameObject droppedItem = eventData.pointerDrag;
      ItemSlot itemSlot = droppedItem.GetComponent<ItemSlot>();
      itemSlot.parentAfterDrag = transform;
    }
}
