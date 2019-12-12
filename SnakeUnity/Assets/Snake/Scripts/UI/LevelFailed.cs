using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFailed : MonoBehaviour
{
    public GameObject title;
    public CanvasGroup reviveField;
    public Image reviveTime;
    
    private bool showWindow;
    private float timer = 3.5f;
    private float timerProcess;
    private float timerDelay = 1;
    private float timerDelayProcess;

    bool gateReviveField;

    [HideInInspector] public bool waitingOff;

    void Update()
    {
        if(showWindow)
        {
            if(timerDelayProcess < timerDelay)
            {
                timerDelayProcess += Time.deltaTime;

                if(timerDelayProcess >= 0.5f)
                {
                    if(gateReviveField)
                    {
                        gateReviveField = false;
                        reviveField.gameObject.SetActive(true);
                        reviveField.alpha = 0;
                    }
                    reviveField.alpha = ((timerDelayProcess - 0.5f) / (timerDelay - 0.5f));
                }

                if(timerDelayProcess >= timerDelay)
                {
                    reviveField.alpha = 1;
                }
            }
            else
            {
                timerProcess += Time.deltaTime;

                float delta = timerProcess / timer;
                delta = Mathf.Clamp01(delta);
                reviveTime.fillAmount = 1 - delta;

                if(timerProcess >= timer)
                {
                    showWindow = false;
                    waitingOff = true;
                }
            }
        }
    }

    public void ShowWindow(bool showReviveButton)
    {
        title.SetActive(true);

        if(showReviveButton)
        {
            showWindow = true;
            gateReviveField = true;

            timerDelayProcess = 0;
            timerProcess = -0.5f;
            reviveTime.fillAmount = 1;

            reviveField.alpha = 0;
            reviveField.interactable = true;
        }
    }

    public void HideWindow()
    {
        title.SetActive(false);
        reviveField.gameObject.SetActive(false);

        showWindow = false;
        gateReviveField = false;
        
        reviveField.alpha = 0;
        reviveField.interactable = false;
    }
}
