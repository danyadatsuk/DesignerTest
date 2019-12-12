using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimator : MonoBehaviour
{
    public TubeAnimator[] tubeBig;
    public TubeAnimator[] tubeSmall;
    
    public bool animationPlay = false;
    public float animationTime = 1f;
    private float animationTimeProcess;
    public AnimationCurve animationFlow;

    private int step = 1;
    private int maxStep = 15;
    private bool nextPart = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(animationPlay)
        {
            if(nextPart)
            {
                switch(step)
                {
                    case 1:  SetStartAnimation(new int[]{9, 10}, new int[]{}); break;
                    case 2:  SetStartAnimation(new int[]{8, 11}, new int[]{}); break;
                    case 3:  SetStartAnimation(new int[]{7, 12}, new int[]{}); break;
                    case 4:  SetStartAnimation(new int[]{6, 13}, new int[]{}); break;
                    case 5:  SetStartAnimation(new int[]{5, 14}, new int[]{}); break;
                    case 6:  SetStartAnimation(new int[]{4, 15}, new int[]{9, 10}); break;
                    case 7:  SetStartAnimation(new int[]{3, 16}, new int[]{8, 11}); break;
                    case 8:  SetStartAnimation(new int[]{2, 17}, new int[]{7, 12}); break;
                    case 9:  SetStartAnimation(new int[]{1, 18}, new int[]{6, 13}); break;
                    case 10: SetStartAnimation(new int[]{0, 19}, new int[]{5, 14}); break;
                    case 11: SetStartAnimation(new int[]{},      new int[]{4, 15}); break;
                    case 12: SetStartAnimation(new int[]{},      new int[]{3, 16}); break;
                    case 13: SetStartAnimation(new int[]{},      new int[]{2, 17}); break;
                    case 14: SetStartAnimation(new int[]{},      new int[]{1, 18}); break;
                    case 15: SetStartAnimation(new int[]{},      new int[]{0, 19}); break;
                }
                nextPart = false;
            }
            
            animationTimeProcess += Time.deltaTime;

            if(animationTimeProcess >= 0.1f * step)
            {
                if(step < maxStep)
                {
                    step++;
                    nextPart = true;
                }
                else
                {
                    animationPlay = false;
                    Manager.startNewLevel = true;
                }
            }

        }
    }

    private void SetStartAnimation(int[] numTubeSmall, int[] numTubeBig)
    {
        for(int i = 0; i < numTubeSmall.Length; i++)
        {
            tubeSmall[numTubeSmall[i]].PlayAnimation();
        }
        for(int i = 0; i < numTubeBig.Length; i++)
        {
            tubeBig[numTubeBig[i]].PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        animationPlay = true;
        nextPart = true;
    }
}
