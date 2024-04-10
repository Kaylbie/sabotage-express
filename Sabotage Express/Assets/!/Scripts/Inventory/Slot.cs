using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : NetworkBehaviour,IDropHandler
{
    public Image image;
    public Color selecetedColor, notSelectedColor;
    private void Awake(){
        Deselect();
    }

    public void Select(){
        Debug.Log("Selected color");
        image.color=selecetedColor;
    }
    public void Deselect(){
        Debug.Log("Deselected color");
        image.color=notSelectedColor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount== 0){
            Item item = eventData.pointerDrag.GetComponent<Item>();
            item.parentAfterDrag=transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
    }
}
