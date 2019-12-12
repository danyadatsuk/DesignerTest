using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float timeShowloadScreen = 1;
    private float timeShowloadScreenProcess;
    private bool showLoadScreen;
    private bool hideLoadScreen;
    public AnimationCurve animationCurve;

    
    void Update()
    {
        if(showLoadScreen)
        {
            timeShowloadScreenProcess += Time.deltaTime;

            canvasGroup.alpha = animationCurve.Evaluate(timeShowloadScreenProcess / timeShowloadScreen);

            if(timeShowloadScreenProcess >= timeShowloadScreen)
                showLoadScreen = false;
        }

        if(hideLoadScreen)
        {
            timeShowloadScreenProcess -= Time.deltaTime;

            canvasGroup.alpha = animationCurve.Evaluate(timeShowloadScreenProcess / timeShowloadScreen);

            if(timeShowloadScreenProcess <= 0)
                hideLoadScreen = false;
        }
    }

    public void ShowLoadScreen()
    {
        showLoadScreen = true;
        timeShowloadScreenProcess = 0;
        canvasGroup.alpha = 0;
    }

    public void HideLoadScreen()
    {
        hideLoadScreen = true;
        timeShowloadScreenProcess = timeShowloadScreen;
        canvasGroup.alpha = 1;
    }
}
