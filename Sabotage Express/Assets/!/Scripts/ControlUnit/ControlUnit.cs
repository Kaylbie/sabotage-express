using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ControlUnit : NetworkBehaviour
{
    [SerializeField] KeypadManager keypadManager;
    [SerializeField] MatchSystemManager matchSystemManager;
    [SerializeField] private GameObject door;
    [SerializeField] private string password;
    private NavMeshObstacle obstacle;
    private NetworkVariable<bool> isAccessGranted = new NetworkVariable<bool>();
    void Start()
    {
        obstacle = door.GetComponent<NavMeshObstacle>();
        isAccessGranted.OnValueChanged += HandleAccessGrantedChanged;

    }
    [ServerRpc(RequireOwnership = false)]
    public void SetDoorStateServerRpc(bool isOpen)
    {
        isAccessGranted.Value = isOpen;
        UpdateDoorState();
    }
    private void HandleAccessGrantedChanged(bool oldValue, bool newValue)
    {
        UpdateDoorState();
    }
    private void UpdateDoorState()
    {
        door.GetComponent<Animator>().SetBool("IsOpen", isAccessGranted.Value);
    }
    private void OnDestroy()
    {
        isAccessGranted.OnValueChanged -= HandleAccessGrantedChanged;
    }
    private void Update()
    {
        password = keypadManager.GetPassword();
        if (matchSystemManager.AccessWasGranted())
        {
            SetDoorStateServerRpc(true);
        }
        else if (keypadManager.AccessGranted() || matchSystemManager.AccessGranted())
        {
            SetDoorStateServerRpc(true);
            door.GetComponent<NavMeshObstacle>().enabled = false;
            door.GetComponent<Animator>().enabled = false;
        }

        //door.GetComponent<Animator>().SetBool("IsOpen", isAccessGranted);
    }

}
