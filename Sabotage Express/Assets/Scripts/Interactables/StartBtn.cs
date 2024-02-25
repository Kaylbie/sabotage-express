using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtn : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Interact(GameObject player)
    {
        Debug.Log("Start button pressed!");
    }
}
