using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePanel : MonoBehaviour
{
    [SerializeField] private Quaternion targetRotation = new Quaternion(0, 0, 0, 1);
    [SerializeField] private Quaternion originalRotation = new Quaternion(0, 0.991444886f, 0, 0.13052626f);
    [SerializeField] public float rotationSpeed = 5.0f;
    [SerializeField] private Panel panel;

    public bool isActivated = false;

    void Update()
    {
        if (!isActivated)
        {
            if (!panel.GetIsRotating())
            {
                Rotate(targetRotation);
            }
            else
            {
                Rotate(originalRotation);
            }
        }
    }
    public void SetIsActivated(bool setActive)
    {
        isActivated = setActive;
    }
    private void Rotate(Quaternion toRotation)
    {
        if (Quaternion.Angle(transform.localRotation, toRotation) > 0.01f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, toRotation, Time.deltaTime * rotationSpeed);
            return;
        }

        transform.localRotation = toRotation;
    }
}
