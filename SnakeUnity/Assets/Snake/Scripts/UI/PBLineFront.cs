using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBLineFront : MonoBehaviour
{
    public FrontPoint[] points;
    private FrontPoint[] pointsWork;

    private void Clear(int maxCell) 
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].gameObject.SetActive(false);
        }

        pointsWork = new FrontPoint[maxCell];
        for(int i = 0; i < maxCell; i++)
        {
            if(i < maxCell - 1)
            {
                pointsWork[i] = points[i];
                pointsWork[i].gameObject.SetActive(false);
            }
            else
            {
                pointsWork[i] = points[points.Length - 1];
                pointsWork[i].gameObject.SetActive(false);

            }
        }
    }
    public void AddCell(int numberCell, int maxCell)
    {
        Clear(maxCell);
        //print("numberCell = " + numberCell);
        for (int i = 0; i < maxCell; i++)
        {
            pointsWork[i].gameObject.SetActive(true);

            if(i < numberCell)
            {
                pointsWork[i].SetImage(true, false);
            }
            else if(i == numberCell)
            {
                pointsWork[i].SetImage(true, true);
            }
            else
            {
                pointsWork[i].SetImage(false, false);
            }
        }
    }

    public void SubtractCell(int numberCell)
    {
        pointsWork[numberCell].HideImage();
            
    }

    
}
