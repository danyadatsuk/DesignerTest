using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Finish finish;
    public GameObject textStart;
    public GameObject textFinish;
    public GameObject textSwipeToPlay;
    

    // Start is called before the first frame update
    public void Initialization(PlayerController playerController, int maxSavingTails)
    {
        finish.Initialization(playerController, maxSavingTails);
    }

    public void ShowText()
    {
        textStart.SetActive(true);
        textSwipeToPlay.SetActive(true);
    }

    public void HideText()
    {
        textSwipeToPlay.SetActive(false);
    }
}
