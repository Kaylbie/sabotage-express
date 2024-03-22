using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] public float fallSpeed = 2.5f;

    public void SetSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    void Update()
    {
        transform.localPosition -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
    }
}
