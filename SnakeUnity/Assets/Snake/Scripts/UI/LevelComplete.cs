using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    public GameObject title;
    public CanvasGroup coinField;
    public ParticleSystem ps;
    public AnimationCurve animate;
    
    private bool showWindow;
    private bool gateParticle;
    private float timer = 2;
    private float timerProcess;


    void Update()
    {
        if(showWindow)
        {
            timerProcess += Time.deltaTime;

            float delta = timerProcess / timer;

            float alpha = Mathf.Lerp(0, 1, animate.Evaluate(delta));
            coinField.alpha = alpha;

            if(delta >= 0.75f && gateParticle)
            {
                gateParticle = false;
                ps.Play();
            }

            if(timerProcess >= timer)
            {
                showWindow = false;
            }
        }
    }

    public void ShowWindow()
    {
        showWindow = true;
        gateParticle = true;

        timerProcess = 0;
        title.SetActive(true);
        coinField.gameObject.SetActive(true);
        coinField.alpha = 0;
    }

    public void HideWindow()
    {
        title.SetActive(false);
        coinField.gameObject.SetActive(false);

        showWindow = false;
    }
}
