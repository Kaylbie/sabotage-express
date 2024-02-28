using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUnit : MonoBehaviour
{
    [SerializeField] KeypadManager keypadManager;
    [SerializeField] MatchSystemManager matchSystemManager;

    [SerializeField] private bool isAccessGranted = false;

    private void Update()
    {
        if (matchSystemManager.AccessWasGranted())
        {
            isAccessGranted = matchSystemManager.AccessGranted();
        }
        else if (keypadManager.AccessGranted() || matchSystemManager.AccessGranted())
        {
            isAccessGranted = true;
        }
    }

}
