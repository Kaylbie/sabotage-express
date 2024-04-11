using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlUnit : MonoBehaviour
{
    [SerializeField] KeypadManager keypadManager;
    [SerializeField] MatchSystemManager matchSystemManager;
    [SerializeField] private GameObject door;
    [SerializeField] private string password;
    [SerializeField] private bool isAccessGranted = false;
    private NavMeshObstacle obstacle;

    void Start()
    {
        obstacle = door.GetComponent<NavMeshObstacle>();
        if (obstacle != null)
        {
            Debug.Log("obstacle found");
        }
    }

    private void Update()
    {
        password = keypadManager.GetPassword();
        if (matchSystemManager.AccessWasGranted())
        {
            isAccessGranted = matchSystemManager.AccessGranted();
        }
        else if (keypadManager.AccessGranted() || matchSystemManager.AccessGranted())
        {
            isAccessGranted = true;
            door.GetComponent<NavMeshObstacle>().enabled = false;
            door.GetComponent<Animator>().enabled = false;
        }

        door.GetComponent<Animator>().SetBool("IsOpen", isAccessGranted);
    }

}
