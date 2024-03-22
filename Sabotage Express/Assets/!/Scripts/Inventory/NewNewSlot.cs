using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class NewNewSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
       if(transform.childCount == 0)
       {
           TempSlot tempSlot = eventData.pointerDrag.GetComponent<TempSlot>();
              tempSlot.parentAfterDrag = transform;
       }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
