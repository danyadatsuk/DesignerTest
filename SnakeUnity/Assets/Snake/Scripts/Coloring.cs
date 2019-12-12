using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coloring : MonoBehaviour
{
    [Header("Background color")]
    public Camera mainCamera;
    public AmplifyOcclusionEffect ssao;

    [Header("Level Color")]
    private TextMeshPro textStartColor;
    private TextMeshPro textFinishColor;
    public Material textLineColor;
    public Material floorColor;
    public Material borderColor;

    [Header("Object Color")]
    public Material objectNetralColor1;
    public Material objectNetralColor2;
    public Material objectNetralColor3;
    public Material objectAssaultColor;

    [Header("Snake Color")]
    public Material snakeColor;
    public Material feedColor;

    private ColorScheme colorSchemeNew;
    private ColorScheme colorSchemeOld;

    
    private bool coloring;
    private bool delaying;
    
    [Header("Timing Coloring")]
    public float timerDelay = 5f;
    private float timerDelayProcess;
    public float timerChangeColor = 3f;
    private float timerChangeColorProcess; 

    public void InitializationOneScheme(ColorScheme cs, GameObject textStart, GameObject textFinish)
    {
        colorSchemeNew = cs;
        textStartColor = textStart.GetComponent<TextMeshPro>();
        textFinishColor = textFinish.GetComponent<TextMeshPro>();
    }

    public void InitializationTwoScheme(ColorScheme csOld, ColorScheme csNew, GameObject textStart, GameObject textFinish)
    {
        colorSchemeOld = csOld;
        colorSchemeNew = csNew;
        textStartColor = textStart.GetComponent<TextMeshPro>();
        textFinishColor = textFinish.GetComponent<TextMeshPro>();
    }

    public void SetColorSchemeForce()
    {
        // Background color
        mainCamera.backgroundColor = colorSchemeNew.cameraColor;
        if(ssao != null)
        {
            ssao.Tint = colorSchemeNew.ssaoColor;
        }
        
        // Level Color
        textStartColor.color = colorSchemeNew.textColor;
        textFinishColor.color = colorSchemeNew.textColor;
        textLineColor.color = colorSchemeNew.textLineColor;
        floorColor.color = colorSchemeNew.floorColor;
        borderColor.color = colorSchemeNew.borderColor;

        // Object Color
        objectNetralColor1.color = colorSchemeNew.objectNetralColor1;
        objectNetralColor2.color = colorSchemeNew.objectNetralColor2;
        objectNetralColor3.color = colorSchemeNew.objectNetralColor3;
        objectAssaultColor.color = colorSchemeNew.objectAssaultColor;

        // Snake Color
        snakeColor.color = colorSchemeNew.snakeColor;
        feedColor.color = colorSchemeNew.feedColor;
    }

    public void SetColorSchemeLerp()
    {
        timerChangeColorProcess = 0;
        timerDelayProcess = 0;
        delaying = true;
    }

    private void Update() 
    {
        if(delaying)
        {
            timerDelayProcess += Time.deltaTime;

            if(timerDelayProcess >= timerDelay)
            {
                delaying = false;
                coloring = true;
            }
        }

        if(coloring)
        {
            timerChangeColorProcess += Time.deltaTime;
            float delta = timerChangeColorProcess / timerChangeColor;
            
            if(delta >= 1)
            {
                coloring = false;
                delta = 1;
            }

            // Background color
            mainCamera.backgroundColor = Vector4.Lerp(colorSchemeOld.cameraColor, colorSchemeNew.cameraColor, delta);
            if(ssao != null)
            {
                ssao.Tint = Vector4.Lerp(colorSchemeOld.ssaoColor, colorSchemeNew.ssaoColor, delta);
            }
            
            // Level Color
            textStartColor.color = Vector4.Lerp(colorSchemeOld.textColor, colorSchemeNew.textColor, delta);
            textFinishColor.color = Vector4.Lerp(colorSchemeOld.textColor, colorSchemeNew.textColor, delta);
            textLineColor.color = Vector4.Lerp(colorSchemeOld.textLineColor, colorSchemeNew.textLineColor, delta);
            floorColor.color = Vector4.Lerp(colorSchemeOld.floorColor, colorSchemeNew.floorColor, delta);
            borderColor.color = Vector4.Lerp(colorSchemeOld.borderColor, colorSchemeNew.borderColor, delta);

            // Object Color
            objectNetralColor1.color = Vector4.Lerp(colorSchemeOld.objectNetralColor1, colorSchemeNew.objectNetralColor1, delta);
            objectNetralColor2.color = Vector4.Lerp(colorSchemeOld.objectNetralColor2, colorSchemeNew.objectNetralColor2, delta);
            objectNetralColor3.color = Vector4.Lerp(colorSchemeOld.objectNetralColor3, colorSchemeNew.objectNetralColor3, delta);
            objectAssaultColor.color = Vector4.Lerp(colorSchemeOld.objectAssaultColor, colorSchemeNew.objectAssaultColor, delta);

            // Snake Color
            snakeColor.color = Vector4.Lerp(colorSchemeOld.snakeColor, colorSchemeNew.snakeColor, delta);
            feedColor.color = Vector4.Lerp(colorSchemeOld.feedColor, colorSchemeNew.feedColor, delta);
        }
    }
}
