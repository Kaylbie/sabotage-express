using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovablePair : Interactable
{
    private Vector3 initialPosition;
    private bool isConnected;
    private const string portTag = "Port";
    [SerializeField] private float dragResponseThreshold = 0.2f;

    private bool isInUse = false;
    private GameObject player;
    private RaycastHit hitInfo;

    public void Update()
    {
        if (isInUse)
        {
            hitInfo = player.GetComponent<PlayerInteract>().hitInfo;
            if (hitInfo.transform == null)
            {
                ResetPosition();
                isInUse = false;
            }
            else
            {
                Vector3 newWorldPosition = hitInfo.point;

                if (!isConnected)
                {
                    transform.position = new Vector3(transform.position.x, newWorldPosition.y, newWorldPosition.z);
                }
                else if (Vector3.Distance(transform.position, newWorldPosition) > dragResponseThreshold)
                {
                    isConnected = false;
                }
            }
        }
        else
        {
            if (!isConnected)
            {
                ResetPosition();
            }
        }
    }

    protected override void Interact(GameObject player)
    {
        if (isInUse)
        {
            isInUse = false;
        }
        else
        {
            isInUse = true;

        }
        if (player != null)
        {
            this.player = player;
        }
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    public void SetInitialPosition(Vector3 newPosition)
    {
        initialPosition = newPosition;
        transform.position = initialPosition;
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(portTag))
        {
            isConnected = true;
            isInUse = false;
            transform.position = other.transform.position;
        }
    }
}
