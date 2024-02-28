using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePair : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraZDistance;
    private Vector3 initialPosition;
    private bool isConnected;
    [SerializeField] bool hasPower;
    private const string portTag = "Port";
    private const float dragResponseThreshold = 2;

    void Start()
    {
        mainCamera = Camera.main;
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }
    void OnMouseDrag()
    {
        Vector3 screenPostiion = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 newWorldPosition = mainCamera.ScreenToWorldPoint(screenPostiion);

        if (!isConnected)
        {
            transform.position = newWorldPosition;
        }
        else if (Vector3.Distance(transform.position, newWorldPosition) > dragResponseThreshold)
        {
            isConnected = false;
        }
    }

    private void OnMouseUp()
    {
        if (!isConnected)
        {
            ResetPosition();
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
        // Debug.Log(initialPosition);
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
            transform.position = other.transform.position;
        }
    }
}
