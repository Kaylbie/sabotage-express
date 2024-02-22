using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    // Start is called before the first frame update
    public string objectName;
    public string objectDescription;
    public int objectID;
    public int objectWeight;
    public int objectValue;
    public bool pickable;
    public int objectQuantity = 1;
    
    void objectData(string name, string description, int id, int weight, int value, int quantity)
    {
        objectName = name;
        objectDescription = description;
        objectID = id;
        objectWeight = weight;
        objectValue = value;
        objectQuantity = quantity;
    }
    void Start()
    {
        
    }

}
