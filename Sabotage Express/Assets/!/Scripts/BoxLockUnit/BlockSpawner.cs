using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BlockSpawner : NetworkBehaviour
{
    public GameObject[] spawnPoints;
    [SerializeField] public GameObject blockParent;
    [SerializeField] public List<Block> blockPrefabs;
    private Queue<GameObject> blockPool = new Queue<GameObject>();
    private Queue<GameObject> fallingPool = new Queue<GameObject>();

    [SerializeField] public List<BlockButton> buttons;
    [SerializeField] public List<Material> colorMaterials;

    public float spawnInterval = 2.0f;
    public float fallingSpeed = 2.0f;
    private float timer;

    [SerializeField] public int scoreToBeat = 10;
    [SerializeField] private GameObject[] errorFeedback;
    [SerializeField] private Material none;
    [SerializeField] private Material error;
    [SerializeField] private Material success;
    private int mistakes = 0;
    private int score = 0;
    private bool gameOver = true;
    [SerializeField] private GameObject door;

    [SerializeField] bool accessGranted = false;
    private NavMeshObstacle obstacle;

    private void Start()
    {
        InitializePool();
        obstacle= door.GetComponent<NavMeshObstacle>();
        
    }

    private void Update()
    {
        if (mistakes >= errorFeedback.Length || score >= scoreToBeat)
        {

            gameOver = true;
            ClearFeedback();
            if (score >= scoreToBeat)
            {
                accessGranted = true;
                ShowSuccess();
                obstacle.enabled = false;
                foreach (GameObject gameObject in fallingPool)
                {
                    ReturnBlockToPool(gameObject);
                }
                mistakes = 0;
                score = 0;
            }
        }

        if (!gameOver)
        {
            timer += Time.deltaTime;
            if (timer > spawnInterval)
            {
                SpawnBlock();
                timer = 0;
            }
        }
        ShowMistakes();
        RequestOpenDoorServerRpc();
    }
    [ServerRpc]
    private void RequestOpenDoorServerRpc()
    {
        door.GetComponent<Animator>().SetBool("IsOpen", accessGranted);

    }

    public bool IsAccessGranted()
    {
        return accessGranted;
    }
    private void ShowMistakes()
    {
        for (int i = 0; i < mistakes; i++)
        {
            errorFeedback[i].GetComponent<Renderer>().material = error;
        }
    }
    private void ClearFeedback()
    {
        foreach (var feedback in errorFeedback)
        {
            feedback.GetComponent<Renderer>().material = none;
        }
    }
    private void ShowSuccess()
    {
        foreach (var feedback in errorFeedback)
        {
            feedback.GetComponent<Renderer>().material = success;
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < blockPrefabs.Count; i++)
        {
            blockPrefabs[i].SetSpeed(fallingSpeed);
            GameObject blockInstance = Instantiate(blockPrefabs[i].gameObject, blockParent.transform);
            blockInstance.SetActive(false);
            blockPool.Enqueue(blockInstance);
        }
    }

    void SpawnBlock()
    {
        if (blockPool.Count > 0)
        {
            GameObject blockToSpawn = blockPool.Dequeue();
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            blockToSpawn.transform.localPosition = spawnPoints[spawnIndex].transform.localPosition;
            int materialSpawnIndex = Random.Range(0, colorMaterials.Count);
            blockToSpawn.GetComponent<Renderer>().material = colorMaterials[materialSpawnIndex];
            blockToSpawn.name = colorMaterials[materialSpawnIndex].name;
            blockToSpawn.SetActive(true);
            fallingPool.Enqueue(blockToSpawn);
        }
    }

    public void ReturnBlockToPool(GameObject block)
    {
        block.SetActive(false);
        blockPool.Enqueue(block);
    }

    public void DestroyBlock(GameObject block)
    {
        if (mistakes < errorFeedback.Length)
        {
            mistakes++;
        }
        fallingPool.Dequeue();
        ReturnBlockToPool(block);
    }

    public void ProcessKeyPress(string keyValue)
    {
        if (gameOver == false && fallingPool.Peek().GetComponent<Renderer>().name.Equals(keyValue))
        {
            score++;
            ReturnBlockToPool(fallingPool.Dequeue());
        }
        if (keyValue.Equals("Start"))
        {
            mistakes = 0;
            score = 0;
            gameOver = false;
            Shuffle(colorMaterials);
            for (int i = 0; i < colorMaterials.Count; i++)
            {
                buttons[i].SetMaterial(colorMaterials[i]);
            }
            ClearFeedback();
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
