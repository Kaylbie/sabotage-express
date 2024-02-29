using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : Interactable
{
    public KeypadManager keypadManager;

    protected override void Interact(GameObject player)
    {
        keypadManager.ScrewRelease();
    }
}
