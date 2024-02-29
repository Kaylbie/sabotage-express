using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    private string currentInput = "";
    [SerializeField] private string correctPassword = "";
    [SerializeField] private int passwordSize = 4;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private GameObject frontPlate;
    [SerializeField] private GameObject screw;

    private bool isAccessGranted = false;

    private void Start()
    {
        for (int i = 0; i < passwordSize; i++)
        {
            correctPassword += Random.Range(0, 10);
        }
    }
    public bool AccessGranted()
    {
        return isAccessGranted;
    }
    public void ProcessKeyPress(string key)
    {
        if (key == "C")
        {
            currentInput = "";
        }
        else
        {
            currentInput += key;
        }

        if (currentInput == correctPassword)
        {
            text.text = "Enter";
            text.color = Color.green;
            currentInput = "";
            isAccessGranted = true;
        }
        else
        {
            text.text = currentInput;
            text.color = Color.white;
        }
        if (currentInput.Length == passwordSize && currentInput != correctPassword)
        {
            text.text = "Error";
            text.color = Color.red;
            currentInput = "";
        }


    }

    public void ScrewRelease()
    {
        Rigidbody rb = frontPlate.AddComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.mass = 10;
        rb.drag = 1;

        HingeJoint hinge = gameObject.AddComponent<HingeJoint>();

        hinge.autoConfigureConnectedAnchor = true;
        hinge.useLimits = true;

        // Add a HingeJoint for the door to swing
        HingeJoint doorHinge = frontPlate.AddComponent<HingeJoint>();
        doorHinge.axis = new Vector3(0, 0, 1); // Set hinge axis to Z-axis
        doorHinge.useLimits = true;

        // Set the hinge anchor to the edge of the door
        // This may need adjustment based on your door model's pivot point
        doorHinge.anchor = new Vector3(-0.425f, -0.43f, 0);

        // Configure the swing limits
        JointLimits limits = new JointLimits();
        limits.min = -180; // Minimum swing angle
        limits.max = 180;  // Maximum swing angle
        doorHinge.limits = limits;

    }
}
