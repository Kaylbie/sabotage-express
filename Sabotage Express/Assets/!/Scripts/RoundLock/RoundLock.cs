using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundLock : MonoBehaviour
{
    [SerializeField] private GameObject spinWall;
    private Quaternion startSpinWallRotation;

    [SerializeField] private GameObject keyBall;
    private Quaternion startKeyBallRotation;

    [SerializeField] private GameObject status;

    [SerializeField] private Material success;
    [SerializeField] private Material fail;
    [SerializeField] private Material clear;

    private float spinWallRotationSpeed = 100f;
    private float keyBallRotationSpeed = 100f;
    private int spinWallDirection = 1;
    [SerializeField] private bool keyBallDirectionToggle = false;
    [SerializeField] private bool startGame = false;
    [SerializeField] private float timeToBeat = 2f;
    [SerializeField] private int rotationChangeCount = 10;
    private int rotationChanged = 0;



    private float gameTimer = 2f;
    [SerializeField] private bool accessGranted = false;

    void Start()
    {
        startSpinWallRotation = spinWall.transform.localRotation;
        startKeyBallRotation = keyBall.transform.localRotation;
        gameTimer = timeToBeat;
    }

    void Update()
    {
        if (startGame)
        {
            status.GetComponent<Renderer>().material = clear;
            gameTimer -= Time.deltaTime;
            // if (gameTimer <= 0)
            // {
            if (rotationChanged >= rotationChangeCount)
            {
                startGame = false;

                spinWall.transform.localRotation = startSpinWallRotation;
                keyBall.transform.localRotation = startKeyBallRotation;

                status.GetComponent<Renderer>().material = success;
                accessGranted = true;
                return;
            }

            spinWall.transform.Rotate(0, spinWallRotationSpeed * spinWallDirection * Time.deltaTime, 0);

            if (Random.Range(0, 100) < 1)
            {
                spinWallDirection *= -1;
                rotationChanged++;
            }

            if (keyBallDirectionToggle)
            {
                keyBall.transform.Rotate(0, keyBallRotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                keyBall.transform.Rotate(0, -keyBallRotationSpeed * Time.deltaTime, 0);
            }
        }
    }

    public void endGame()
    {
        startGame = false;

        spinWall.transform.localRotation = startSpinWallRotation;
        keyBall.transform.localRotation = startKeyBallRotation;

        status.GetComponent<Renderer>().material = fail;
    }

    public void ButtonPressed()
    {
        if (!accessGranted)
        {
            if (!startGame)
            {
                startGame = true;
                gameTimer = timeToBeat;
                rotationChanged = 0;
            }
            else
            {
                keyBallDirectionToggle = !keyBallDirectionToggle;
            }
        }
    }

    public bool IsAccessGranted()
    {
        return accessGranted;
    }
}
