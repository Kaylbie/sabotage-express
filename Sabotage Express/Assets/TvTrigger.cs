using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvTrigger : MonoBehaviour
{
    private BoxCollider boxCollider;

    public GameObject tv;

    private TVScript tvScript;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        tvScript = tv.GetComponent<TVScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tvScript.PauseVideoServerRpc();
        }
    }
}
