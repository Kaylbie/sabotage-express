using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class ItemScript : ScriptableObject 
{

public ItemType type;
public int itemID;
public string itemName;
public ActionType actionType;
public Sprite image;

public bool stackable = true;



public enum ItemType{
    Gun,
    Meale,
    Granade,
    loot
}
public enum ActionType{
    Shoot,
    Throw,
    Drop,
    Use
}


}
