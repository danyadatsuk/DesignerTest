using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{
    public Transform snakeHead;
    public Collider snakeHeadCollider;
    public float partDiameter;

    public List<Transform> snakePart = new List<Transform>();
    public List<Vector3> positions = new List<Vector3>();

    [HideInInspector]
    public PlayerController playerController;


    public float speedRotationPart = 30f;
    public float speedGrowth = 5;

    public SnakeChunk chunkPrefab;
    public Manager manager;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        positions.Add(snakeHead.position);

        chunkPrefab.Instantiate();
    }

    void Update()
    {
        float distance = ((Vector3)snakeHead.position - positions[0]).magnitude;

        if(distance > partDiameter)
        {
            Vector3 direction = ((Vector3)snakeHead.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * partDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= partDiameter;
        }

        for(int i = 0; i < snakePart.Count; i++)
        {
            snakePart[i].position = Vector3.Lerp(positions[i + 1], positions[i], distance / partDiameter);
        }

        //Вращение головы и частей червяка   
        snakeHead.LookAt(playerController.DummyTarget);
        if(snakePart.Count > 0)
        {
            snakePart[0].LookAt(snakeHead);
        }
            
        for(int i = 0; i < snakePart.Count - 1; i++)
        {
            Vector3 relativePos = snakePart[i + 1].position - snakePart[i].position;
            if(relativePos != Vector3.zero)
            {
                var newRot = Quaternion.LookRotation(relativePos);
                snakePart[i + 1].rotation = Quaternion.Lerp(snakePart[i + 1].rotation, newRot, Time.deltaTime * speedRotationPart);
            }
        }
        for(int i = 0; i < snakePart.Count; i++)
        {
            float curScale = snakePart[i].transform.localScale.x;
            if(curScale < 1)
            {
                curScale += Time.deltaTime * speedGrowth;
                curScale = Mathf.Clamp01(curScale);
                snakePart[i].transform.localScale = new Vector3(curScale, curScale, curScale);
            }
        }
    }

    public void AddPart()
    {
        Transform part = transform.GetChild(snakePart.Count + 1);
        part.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        part.gameObject.SetActive(true);
        part.position = positions[positions.Count - 1];
        part.transform.rotation = Quaternion.identity;
        part.transform.localPosition = Vector3.zero;
        
        part.GetComponent<SnakePartDetectCol>().ID = snakePart.Count;

        snakePart.Add(part);
        positions.Add(part.position);

        manager.UpdateProgressBar(1, true);
    }

    public void DestroyParts(int IDpoint, bool updateProgressBar)
    {
        for(int i = IDpoint; i < snakePart.Count; i++)
        {
            snakePart[i].GetComponent<BoxCollider>().isTrigger = false;
            snakePart[i].gameObject.AddComponent<Rigidbody>();
            snakePart[i].parent = null;
            snakePart[i].GetComponent<SnakePartDetectCol>().Refile();
        }
        
        snakePart.RemoveRange(IDpoint, snakePart.Count - IDpoint);
        positions.RemoveRange(IDpoint + 1, positions.Count - 1 - IDpoint);

        if(updateProgressBar)
        {
            manager.UpdateProgressBar(IDpoint, false);
        }
    }

    public void DestroyAll()
    {
        chunkPrefab.gameObject.SetActive(true);
        chunkPrefab.transform.parent = null;
        snakeHead.GetComponent<MeshRenderer>().enabled = false;
        snakeHeadCollider.GetComponent<Collider>().enabled = false;

        DestroyParts(0, true);
        Manager.calculateFailingEnd = true;
        Manager.calculateFailingRevive = true;
    }

    public void ReviveSnake()
    {
        chunkPrefab.gameObject.SetActive(false);
        chunkPrefab.prefab.ResetParameters();
        chunkPrefab.transform.parent = snakeHead;
        chunkPrefab.transform.localPosition = Vector3.zero;
        chunkPrefab.transform.localRotation = Quaternion.identity;

        snakeHead.GetComponent<MeshRenderer>().enabled = true;
        snakeHeadCollider.GetComponent<Collider>().enabled = true;
    }

    public void RememberPartNumber()
    {
        manager.SetRememberCountPart(snakePart.Count);
    }
}
