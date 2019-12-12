using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region private main var
    private float screenWidth;
    private float screenHeight;
    
    private Vector3 prevMousePos;
    private Vector3 curMousePos;
    private Vector3 offsetMousePos;
    private Vector3 prevOffsetMousePos;

    private Vector3 newPositionObject;
    private Vector3 prevPositionObject;

    private float minLimitPositionToZ;
    private float maxLimitPositionToZ;

    private Vector3 upSpeedSum;
    #endregion

    #region inspector main var
    public float sensitiveX = 1f;
    public float sensitiveZ = 2f;
    public float reactionSpeed = 4f;
    public float upSpeed = 4f;
    public Transform DummyTarget;
    #endregion
    
    #region animation var
    [Header("Настройки для анимации финиша")]
    public AnimationCurve movingX;
    public AnimationCurve movingY;
    public AnimationCurve movingZ;
    private float distanceJumpX;
    public float distanceJumpY = 1;
    private float distanceJumpZ;
    public float timeMoving = 1f;
    public float factorMovingDeep = 4.5f;
    private float timeMovingProcess;
    private bool animationHideHole;
    private Vector3 startJumpPos;
    #endregion

    public int savingTails = 0;
    public int currentSavingTails = 0;
    private int maxSavingTails = 0;
    private float maxYDive;
    private float tailDiameter;
    private float animationOffsetYDive = 0;

    [Header("Настройки для анимации финиша")]
    public CameraFollow mainCamera;


    [HideInInspector]
    public bool playable = false;
    private float globalTime;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        newPositionObject = transform.position;
        prevPositionObject = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(playable)
        {
            if(Input.GetMouseButtonDown(0))
            {
                // Вычисляем позицию пальца на экране
                Vector3 screenPos = Input.mousePosition;
                curMousePos = new Vector3(screenPos.x / screenWidth, 0, screenPos.y / screenHeight);

                // Сохраним позицию
                prevMousePos = curMousePos;
            }

            if(Input.GetMouseButton(0))
            {
                // Вычисляем позицию пальца на экране
                Vector3 screenPos = Input.mousePosition;
                curMousePos = new Vector3(screenPos.x / screenWidth, 0, screenPos.y / screenHeight);
                
                

                // Если позиция пальца не соответствует позиции в прошлом кадре - засчитываем смещение
                offsetMousePos = curMousePos - prevMousePos;
                // Домножаем смещение на чувствительность перемещения
                offsetMousePos = new Vector3(offsetMousePos.x * sensitiveX, 
                                            offsetMousePos.y, 
                                            offsetMousePos.z * sensitiveZ);
                
                // Добавим прошлое смещение
                offsetMousePos += prevOffsetMousePos;
            }

            if(Input.GetMouseButtonUp(0))
            {
                prevOffsetMousePos = offsetMousePos;
            }

            globalTime += Time.deltaTime;
            upSpeedSum = new Vector3(0, 0, globalTime * upSpeed);
            newPositionObject = offsetMousePos + prevPositionObject + upSpeedSum;
        
            // Расчитываем плавное перемещение игрока
            transform.position = Vector3.Lerp(transform.position, newPositionObject, Time.deltaTime * reactionSpeed);
            
            // Ограничение движения по ширине дороги, а также нельзя уползти далеко вперед за экран и назад
            Vector3 limitationPosition = transform.position;
            minLimitPositionToZ = mainCamera.transform.position.z + 0.6f;
            maxLimitPositionToZ = mainCamera.transform.position.z + 12.6f;
            if(Mathf.Abs(limitationPosition.x) >= 2.1f)
            {
                transform.position = new Vector3(Mathf.Clamp(limitationPosition.x, -2.1f, 2.1f),
                                                limitationPosition.y,
                                                limitationPosition.z);
            }
            if(limitationPosition.z < minLimitPositionToZ || limitationPosition.z > maxLimitPositionToZ)
            {
                transform.position = new Vector3(limitationPosition.x,
                                                limitationPosition.y,
                                                Mathf.Clamp(limitationPosition.z, minLimitPositionToZ, maxLimitPositionToZ));
            }

            

            // Пустышка, за которой следит голова змейки
            DummyTarget.position = newPositionObject;

        }

        if(animationHideHole)
        {
            if(timeMovingProcess < timeMoving)
            {
                timeMovingProcess += Time.deltaTime;
            
                // Анимация прыжка цели (за которой следит голова змейки)
                float animationOffsetX = movingX.Evaluate((timeMovingProcess + 0.05f) / timeMoving) * distanceJumpX;
                float animationOffsetY = movingY.Evaluate((timeMovingProcess + 0.05f) / timeMoving) * distanceJumpY;
                float animationOffsetZ = movingZ.Evaluate((timeMovingProcess + 0.05f) / timeMoving) * distanceJumpZ;
                Vector3 addOffsetPos = new Vector3(animationOffsetX, animationOffsetY, animationOffsetZ);
                DummyTarget.position = startJumpPos + addOffsetPos;
                

                // Анимация прыжка змеики
                animationOffsetX = movingX.Evaluate(timeMovingProcess / timeMoving) * distanceJumpX;
                animationOffsetY = movingY.Evaluate(timeMovingProcess / timeMoving) * distanceJumpY;
                animationOffsetZ = movingZ.Evaluate(timeMovingProcess / timeMoving) * distanceJumpZ;
                addOffsetPos = new Vector3(animationOffsetX, animationOffsetY, animationOffsetZ);
                transform.position = startJumpPos + addOffsetPos;

            }
            
            if(timeMovingProcess > timeMoving)
            {
                if(animationOffsetYDive < maxYDive)
                {
                    timeMovingProcess += Time.deltaTime;

                    // Анимация прыжка цели (за которой следит голова змейки)
                    float animationOffsetY = ((timeMovingProcess + 0.05f - timeMoving) / (timeMoving) * factorMovingDeep) - 0.65f;
                    DummyTarget.position = new Vector3(DummyTarget.position.x, -animationOffsetY, DummyTarget.position.z);

                    // Анимация прыжка змеики
                    animationOffsetYDive = ((timeMovingProcess - timeMoving) / (timeMoving) * factorMovingDeep) - 0.65f;
                    transform.position = new Vector3(transform.position.x, -animationOffsetYDive, transform.position.z);

                    float finalDive = animationOffsetYDive + 0.5f - 0.75f;
                    if(finalDive >= tailDiameter * (currentSavingTails + 1))
                    {
                        currentSavingTails += 1;
                    }
                }
                else
                {
                    // Подводим итоги
                    if(maxSavingTails <= currentSavingTails)
                    {
                        // Level Complete
                        Manager.calculateSuccessEnd = true;
                        animationHideHole = false;

                        Manager.sendEventSuccessLevel = true;
                    }
                    else
                    {
                        Manager.calculateLackParts = true;
                        Manager.calculateFailingEnd = true;
                        Manager.calculateFailingRevive = false;
                    }
                }
            }
        }
    }

    public void SetStartPosition(float newPositionX = 0.0f, float newPositionY = 0.65f, float newPositionZ = 0.0f)
    {
        transform.position = new Vector3(newPositionX, newPositionY, newPositionZ + 3f);
        GetComponent<SnakeTail>().snakeHead.localRotation = Quaternion.identity;
        DummyTarget.localPosition = new Vector3(0, 0, 2.5f);

        globalTime = 0;
        newPositionObject = transform.position;
        prevPositionObject = transform.position;
        prevMousePos = Vector3.zero;
        curMousePos = Vector3.zero;
        offsetMousePos = Vector3.zero;
        prevOffsetMousePos = Vector3.zero;
        upSpeed = 1.5f;

        currentSavingTails = 0;
        savingTails = 0;

        timeMovingProcess = 0;
        animationOffsetYDive = 0;

        GetComponent<SnakeTail>().snakeHeadCollider.enabled = true;
    }

    public void FirstStart(Vector3 mousePosition, int maxTails)
    {
        Vector3 screenPos = mousePosition;
        curMousePos = new Vector3(screenPos.x / screenWidth, 0, screenPos.y / screenHeight);

        prevMousePos = curMousePos;

        maxSavingTails = maxTails;
    }

    public void StopControlFinish(Vector3 targetToJump)
    {
        animationHideHole = true;
        playable = false;
        upSpeed = 0;
        
        startJumpPos = transform.position;
        distanceJumpX = targetToJump.x - startJumpPos.x;
        distanceJumpZ = targetToJump.z - startJumpPos.z;

        GetComponent<SnakeTail>().snakeHeadCollider.enabled = false;

        savingTails = GetComponent<SnakeTail>().snakePart.Count;
        tailDiameter = GetComponent<SnakeTail>().partDiameter;
        maxYDive = tailDiameter * savingTails + 0.75f;
    }

    public void StopControlDestroy()
    {
        playable = false;
        upSpeed = 0;
    }
}
