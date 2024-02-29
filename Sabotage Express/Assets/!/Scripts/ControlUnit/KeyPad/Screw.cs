using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    public KeypadManager keypadManager;

    private void OnMouseUpAsButton()
    {
        keypadManager.ScrewRelease();
    }
}
