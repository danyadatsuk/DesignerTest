using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    private PlayerController playerController;  
    public Transform holePoint;
    public TextMeshPro counter;
    public FinishAnimator finishAnimator;
    public DoorAnimator doorAnimator;
    [HideInInspector] public int maxSavingTails;

    public void Initialization(PlayerController pc, int mst)
    {
        playerController = pc;
        maxSavingTails = mst;

        counter.text = "0/" + mst;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(!other.CompareTag("Player")) return;

        playerController.StopControlFinish(holePoint.position);
        playerController.mainCamera.StopCameraFinish(transform.position);

        Manager.calculateParts = true;
        Manager.calculateResidueVar = true;
    }

    public void UpdateCounter(int currentSavingTails)
    {
        counter.text = currentSavingTails + "/" + maxSavingTails;

        if(currentSavingTails > maxSavingTails)
        {
            counter.color = new Color(0.456f, 0.905f, 0.123f, 1f);
        }
    }

    public void SetLackCounter()
    {
        counter.color = new Color(1, 0.098f, 0.098f, 1);
    }

    public void ActivateCounter(bool activate)
    {
        counter.gameObject.SetActive(activate);
    }

    public void FinishAnimator()
    {
        finishAnimator.PlayAnimation();
    }

    public void DoorAnimator()
    {
        doorAnimator.activate = true;
    }
}
