using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainWagonGen : MonoBehaviour
{
    [SerializeField] public GameObject wagonPrefab;
    [SerializeField] public GameObject spawnOffset;
    [SerializeField] public GameObject spawnTrigger;
    [SerializeField] public GameObject wagons;
    [SerializeField] public bool spawnWagon = false;

    void Update()
    {
        if (spawnWagon)
        {
            SpawnWagon();
            spawnWagon = false;
        }
    }

    public void SpawnWagon()
    {
        GameObject spawnedWagon = Instantiate(wagonPrefab, spawnOffset.transform.localPosition, Quaternion.identity);
        spawnedWagon.transform.SetParent(wagons.transform, false);
        spawnWagon = false;
        Vector3 temp = spawnOffset.transform.localPosition;
        spawnOffset.transform.localPosition = new Vector3(temp.x - 21, temp.y, temp.z);
        temp = spawnTrigger.transform.localPosition;
        spawnTrigger.transform.localPosition = new Vector3(temp.x - 21, temp.y, temp.z);
    }

}
