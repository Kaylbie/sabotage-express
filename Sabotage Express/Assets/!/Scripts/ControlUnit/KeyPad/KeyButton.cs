using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyButton : MonoBehaviour
{
    public KeypadManager keypadManager;
    [SerializeField] private TextMeshPro buttonText;
    public string keyValue;

    public float pressedScaleZ = 0.2f;
    public float animationSpeed = 25f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isPressed = false;
    private void Start()
    {
        if (keypadManager == null)
        {
            keypadManager = FindObjectOfType<KeypadManager>();
        }
        buttonText.text = keyValue;

        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * pressedScaleZ);
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, isPressed ? targetScale : originalScale, Time.deltaTime * animationSpeed);
    }

    void OnMouseDown()
    {
        isPressed = true;
    }

    void OnMouseUp()
    {
        isPressed = false;
    }

    private void OnMouseUpAsButton()
    {
        keypadManager.ProcessKeyPress(keyValue);
    }
}
