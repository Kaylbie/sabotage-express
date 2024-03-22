using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : Interactable
{

    public BlockSpawner blockSpawner;
    public string keyValue;
    public float pressedScaleZ = 0.2f;
    public float animationSpeed = 25f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isPressed = false;

    private void Start()
    {
        if (blockSpawner == null)
        {
            blockSpawner = FindObjectOfType<BlockSpawner>();
        }

        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * pressedScaleZ);
    }
    public void SetKeyValue(string newValue)
    {
        keyValue = newValue;
    }
    public void SetMaterial(Material newMaterial)
    {
        transform.gameObject.GetComponent<Renderer>().material = newMaterial;
        SetKeyValue(newMaterial.name);
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, isPressed ? targetScale : originalScale, Time.deltaTime * animationSpeed);
        Vector3 currentTargetScale = isPressed ? targetScale : originalScale;

        if (Vector3.Distance(transform.localScale, currentTargetScale) < 0.001f)
        {
            if (isPressed)
            {
                isPressed = false;
            }
        }
    }
    private void OnMouseDown()
    {
        // blockSpawner.ProcessKeyPress(keyValue);
        //     isPressed = true;
    }
    protected override void Interact(GameObject player)
    {
        blockSpawner.ProcessKeyPress(keyValue);

    }
}
