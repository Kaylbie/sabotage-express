using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : Interactable
{
    public GameObject enemy;
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
        
        //SceneManager.LoadScene("Wagon", LoadSceneMode.Additive);
    }
}
