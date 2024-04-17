using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] public MovableLever movableLever;
    [SerializeField] public bool isActivated = false;
    [SerializeField] private RotatePanel rotatePanel;
    public void Start()
    {
        movableLever.SetInitialPosition(movableLever.GetPosition());
    }
    public void Update()
    {
        rotatePanel.SetIsActivated(movableLever.GetIsConnected());
        isActivated = movableLever.GetIsConnected();
    }

    // public bool GetIsActivated()
    // {
    //     return isActivated;
    // }
}
