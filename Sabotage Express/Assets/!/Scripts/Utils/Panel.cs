using System.Collections;
using UnityEngine;

public class Panel : Interactable
{
    private bool isRotating = false;

    public bool GetIsRotating()
    {
        return isRotating;
    }

    protected override void Interact(GameObject player)
    {
        isRotating = !isRotating;
    }
}
