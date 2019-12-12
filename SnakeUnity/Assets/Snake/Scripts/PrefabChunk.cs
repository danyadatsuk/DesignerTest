using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabChunk : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] chunk;
    // Start is called before the first frame update
    
    public void Instantiate()
    {
        chunk = new GameObject[transform.childCount];
        for (int i = 0; i < chunk.Length; i++)
        {
            chunk[i] = transform.GetChild(i).gameObject;
        }
    }

    public void ResetParameters()
    {
        for (int i = 0; i < chunk.Length; i++)
        {
            transform.GetChild(i).transform.localPosition = Vector3.zero;
            transform.GetChild(i).transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }
}
