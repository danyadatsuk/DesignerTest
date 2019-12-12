using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBLineBack : MonoBehaviour
{
    public GameObject[] points;

    private void Clear() 
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].SetActive(false);
        }
    }
    public void SetCountEmptyCell(int maxCell)
    {
        Clear();
        int curCell = maxCell - 2;

        for (int i = 0; i < curCell; i++)
        {
            points[i].SetActive(true);
        }
    }
}
