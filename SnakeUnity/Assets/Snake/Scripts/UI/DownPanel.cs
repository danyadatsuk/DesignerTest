using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPanel : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float timeShowDownPanel = 1;
    private float timeShowDownPanelProcess;
    private bool showDownPanel;
    private bool hideDownPanel;
    public AnimationCurve animationCurve;

    [Header("Переменные для окна прокачки")]
    public GameObject upgradeWindow;
    
    void Update()
    {
        if(showDownPanel)
        {
            timeShowDownPanelProcess += Time.deltaTime;

            canvasGroup.alpha = animationCurve.Evaluate(timeShowDownPanelProcess / timeShowDownPanel);

            if(timeShowDownPanelProcess >= timeShowDownPanel)
                showDownPanel = false;
        }

        if(hideDownPanel)
        {
            timeShowDownPanelProcess -= Time.deltaTime;

            canvasGroup.alpha = animationCurve.Evaluate(timeShowDownPanelProcess / timeShowDownPanel);

            if(timeShowDownPanelProcess <= 0)
                hideDownPanel = false;
        }
    }

    public void ShowDownPanel()
    {
        showDownPanel = true;
        timeShowDownPanelProcess = 0;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
    }

    public void HideDownPanel()
    {
        hideDownPanel = true;
        timeShowDownPanelProcess = timeShowDownPanel;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = false;
    }

    public void ShowUpgradePanel()
    {
        upgradeWindow.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        Manager.thirdWindow = true;
    }

    public void HideUpgradePanel()
    {
        upgradeWindow.SetActive(false);
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        Manager.thirdWindow = false;
    }
}
