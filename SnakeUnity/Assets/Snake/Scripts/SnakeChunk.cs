using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeChunk : MonoBehaviour
{
    [HideInInspector]
    public PrefabChunk prefab;
    // Start is called before the first frame update
    public void Instantiate()
    {
        prefab = transform.GetChild(0).GetComponent<PrefabChunk>();
        prefab.Instantiate();
    }
}
