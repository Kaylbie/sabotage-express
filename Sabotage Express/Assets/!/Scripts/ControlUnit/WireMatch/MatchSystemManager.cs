using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MatchSystemManager : MonoBehaviour
{
    [SerializeField] public List<Material> colorMaterials;
    [SerializeField] private TextMeshPro controllerScreen;
    private List<MatchEntity> matchEntities;
    private int targetMatchCount;
    private int currentMatchCount = 0;

    private int firstPair = 0;
    private int secondPair = 0;

    private bool isAccessGranted = false;
    private bool prevIsAccessGranted = false;


    public void Start()
    {
        matchEntities = transform.GetComponentsInChildren<MatchEntity>().ToList();
        targetMatchCount = matchEntities.Count;
        var pair = GetRandomPair();
        firstPair = pair.Item1;
        secondPair = pair.Item2;

        SetEntityColors();
        RandomizeMovablePairPlacement();
    }


    public bool AccessGranted()
    {
        return isAccessGranted;
    }
    public bool AccessWasGranted()
    {
        return prevIsAccessGranted;
    }
    public (int, int) GetRandomPair()
    {
        if (targetMatchCount < 2)
        {
            Debug.LogError("targetMatchCount must be at least 2 to form a pair.");
            return (0, 0);
        }
        int firstIndex = Random.Range(0, targetMatchCount);
        int secondIndex;

        do
        {
            secondIndex = Random.Range(0, targetMatchCount);
        } while (secondIndex == firstIndex);

        return (firstIndex, secondIndex);
    }

    void SetEntityColors()
    {
        int firstPairColorIndex = 0;
        Shuffle(colorMaterials);
        for (int i = 0; i < matchEntities.Count; i++)
        {
            if (secondPair != i)
            {
                if (firstPair == i)
                {
                    firstPairColorIndex = i;
                }
                matchEntities[i].SetMaterialToPairs(colorMaterials[i]);
            }
        }
        matchEntities[secondPair].SetMaterialToPairs(colorMaterials[firstPairColorIndex]);
    }
    void RandomizeMovablePairPlacement()
    {
        List<Vector3> movablePairPosition = new List<Vector3>();
        for (int i = 0; i < matchEntities.Count; i++)
        {
            movablePairPosition.Add(matchEntities[i].GetMovablePairPosition());
        }
        Shuffle(movablePairPosition);

        Vector3 secondPairPortPossition = matchEntities[secondPair].port.transform.position;

        for (int i = 0; i < matchEntities.Count; i++)
        {

            matchEntities[i].SetMovablePairPosition(movablePairPosition[i]);
        }
        for (int i = 0; i < matchEntities.Count; i++)
        {
            if (secondPair != i)
            {
                if (firstPair == i)
                {
                    matchEntities[i].ConnectToPort(secondPairPortPossition);
                }
                else
                {
                    matchEntities[i].ConnectToPort();
                }
            }
        }
    }
    public void NewMatchRecord(bool matchConnected)
    {
        if (matchConnected)
        {
            currentMatchCount++;
        }
        else
        {
            currentMatchCount--;
        }

        if (currentMatchCount == targetMatchCount)
        {
            controllerScreen.text = "Enter";
            controllerScreen.color = Color.green;
            isAccessGranted = true;
            prevIsAccessGranted = true;
        }
        else
        {
            controllerScreen.text = "";
            controllerScreen.color = Color.white;
            if (prevIsAccessGranted)
            {
                isAccessGranted = false;
            }
        }

    }

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
