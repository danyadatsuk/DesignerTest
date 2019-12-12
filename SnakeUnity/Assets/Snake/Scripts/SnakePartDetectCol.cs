using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnakePartDetectCol : MonoBehaviour
{
    public int ID;
    private SnakeTail snakeTail;
    private bool gate = true;
    private bool delayReset = false;
    private float timer;
    private Vector3 curScale;
    void Start()
    {
        snakeTail = transform.parent.GetComponent<SnakeTail>();
    }

    private void Update() 
    {
        if(delayReset)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                float newScale = 2 - timer;
                Vector3 finalScale = new Vector3(newScale * curScale.x, newScale * curScale.y, newScale * curScale.z);
                transform.localScale = finalScale;
                if(timer >= 2)
                {
                    delayReset = false;
                    Destroy(transform.gameObject.GetComponent<Rigidbody>());
                    transform.parent = snakeTail.transform;
                    gameObject.SetActive(false);

                    transform.localScale = Vector3.one;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gate)
        {
            if(!other.CompareTag("Assault")) return;

            if(snakeTail.snakePart.Count > ID)
            {
                snakeTail.DestroyParts(ID, true);
                gate = false;
            }
        }
    }

    public void Refile()
    {
        delayReset = true;
        curScale = transform.lossyScale;
        
        timer = 0;
    }
}
