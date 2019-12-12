using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public PBLineBack lineBack;
    public PBLineFront lineFront;

    public void SetCountEmptyCell(int maxCell)
    {
        lineBack.SetCountEmptyCell(maxCell);
    }

    public void AddCell(int numberCell, int maxCell)
    {
        lineFront.AddCell(numberCell, maxCell);
    }

    public void SubtractCell(int numberCell)
    {
        lineFront.SubtractCell(numberCell);
    }
}
