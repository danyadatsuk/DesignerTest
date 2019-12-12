using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpPanel : MonoBehaviour
{
    public TextMeshProUGUI levelCounter;
    public TextMeshProUGUI coinCounter;
    private int goldCount;
    private int goldNow;


    private bool calculateGold;
    private float timer = 1;
    private float timerProcess;
    private float timerDelay = 3.3f;
    private float timerDelayProcess;

    public void Initialization(int gold, int curLevel)
    {
        levelCounter.text = "Stage " + (curLevel + 1);
        coinCounter.text = gold.ToString();
    }

    void Update()
    {
        if(calculateGold)
        {
            if(timerDelayProcess < timerDelay)
            {
                timerDelayProcess += Time.deltaTime;
            }
            else
            {
                timerProcess += Time.deltaTime;

                float addCoins = Mathf.Lerp(0, goldCount, timerProcess / timer);
                coinCounter.text = (goldNow + (int)addCoins).ToString();

                if(timerProcess >= timer)
                {
                    calculateGold = false;
                }
            }
        }
    }

    public void UpdateCoinBar(int addCoin, int goldAll)
    {
        goldCount = addCoin;
        goldNow = goldAll;

        calculateGold = true;
        timerProcess = 0;

        timerDelayProcess = 0;
    }
}
