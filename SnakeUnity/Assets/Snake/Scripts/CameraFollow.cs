using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speedMoveCamera = 1f;
    private Vector3 curPos;
    private bool stopingDestroy = false;
    private bool stopingFinish = false;
    private bool positionToStart = false;


    [Header("Анимация камеры при поражении")]
    public AnimationCurve stopAnimationDestroy;
    public float timeAnimationDestroy = 1.5f;
    private float timeAnimationDestroyProcess;

    [Header("Анимация камеры при финишировании")]
    public AnimationCurve stopAnimationFinish;
    public float timeAnimationFinish = 1.5f;
    private float timeAnimationFinishProcess;
    private Vector3 positionCameraStart;
    private Vector3 positionCameraFinish;

    [Header("Анимация камеры при подлете к старту")]
    public AnimationCurve toAnimationStart;
    public float timeAnimationStart = 1.5f;
    private float timeAnimationStartProcess;
    private Vector3 positionCameraNow;
    private Vector3 positionCameraNewStart;

    [HideInInspector]
    public bool playable = false;

    // Start is called before the first frame update
    void Start()
    {
        curPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(stopingDestroy)
        {
            timeAnimationDestroyProcess += Time.deltaTime;
            
            speedMoveCamera = stopAnimationDestroy.Evaluate(timeAnimationDestroyProcess / timeAnimationDestroy);

            if(timeAnimationDestroyProcess >= timeAnimationDestroy)
            {
                stopingDestroy = false;
                playable = false;
            }
        }

        if(stopingFinish)
        {
            timeAnimationFinishProcess += Time.deltaTime;

            float softMove = stopAnimationFinish.Evaluate(timeAnimationFinishProcess / timeAnimationFinish);
            transform.position = Vector3.Lerp(positionCameraStart, positionCameraFinish, softMove);
            
            if(timeAnimationFinishProcess >= timeAnimationFinish)
            {
                stopingFinish = false;
            }
        }

        if(positionToStart)
        {
            timeAnimationStartProcess += Time.deltaTime;

            float softMove = toAnimationStart.Evaluate(timeAnimationStartProcess / timeAnimationStart);
            transform.position = Vector3.Lerp(positionCameraNow, positionCameraNewStart, softMove);

            if(timeAnimationStartProcess >= timeAnimationStart)
            {
                positionToStart = false;
                Manager.activateFirstTouch = true;
            }
        }

        if(playable)
        {
            // Основное движение камеры
            curPos = transform.position;
            transform.position = new Vector3(curPos.x, curPos.y, curPos.z + (speedMoveCamera * Time.deltaTime));
        }
    }

    public void StopCameraDestroy()
    {
        stopingDestroy = true;
        timeAnimationDestroyProcess = 0;
    }

    public void StopCameraFinish(Vector3 finishPosition)
    {
        stopingFinish = true;
        timeAnimationFinishProcess = 0;
        playable = false;

        positionCameraStart = transform.position;
        float positionZ = finishPosition.z - 3;
        positionCameraFinish = new Vector3(positionCameraStart.x, positionCameraStart.y, positionZ);
    }

    public void SetCameraToStartPosition(float newPosition)
    {
        positionToStart = true;
        timeAnimationStartProcess = 0;

        positionCameraNow = transform.position;
        positionCameraNewStart = new Vector3(0, 7.55f, newPosition);
    }
}
