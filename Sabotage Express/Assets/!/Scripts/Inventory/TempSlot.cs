using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TempSlot : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentAfterDrag;
    public Image image;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag"+transform.parent);
        image.raycastTarget = false;
        parentAfterDrag = transform.parent.parent.parent;
        
        transform.SetParent(transform.parent.parent);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }


}
