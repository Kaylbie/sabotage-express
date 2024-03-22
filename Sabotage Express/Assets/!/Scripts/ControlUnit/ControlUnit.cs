using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUnit : MonoBehaviour
{
    [SerializeField] KeypadManager keypadManager;
    [SerializeField] MatchSystemManager matchSystemManager;
    [SerializeField] private GameObject door;
    [SerializeField] private string password;
    [SerializeField] private bool isAccessGranted = false;

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
        }

        door.GetComponent<Animator>().SetBool("IsOpen", isAccessGranted);
    }

}
