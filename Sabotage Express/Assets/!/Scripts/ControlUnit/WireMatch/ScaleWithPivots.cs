using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ScaleWithPivots : MonoBehaviour
{
    public GameObject start;
    public GameObject end;

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        UpdateTransformForScale();
    }

    void Update()
    {
        if (start.transform.hasChanged || end.transform.hasChanged)
        {
            UpdateTransformForScale();
        }
    }
    private void UpdateTransformForScale()
    {
        float distance = Vector3.Distance(start.transform.localPosition, end.transform.localPosition);
        transform.localScale = new Vector3(initialScale.x, distance / 2f, initialScale.z);

        Vector3 middlePoint = (start.transform.position + end.transform.position) / 2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (end.transform.position - start.transform.position);
        transform.up = rotationDirection;
    }
}
