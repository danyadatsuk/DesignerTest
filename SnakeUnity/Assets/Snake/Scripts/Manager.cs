using System.Collections;
using System.Collections.Generic;
//using Snake.Scripts.Analytics;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
   // private IAnalyticsController analyticsController;
    private int curLevelEventPassed = -1;
    
    #region config levels
    public LevelsBase levelBase;
    #endregion

    #region interface
    public UpPanel upPanel;
    public LoadingScreen loadingScreen;
    public LevelComplete levelComplete;
    public LevelFailed levelFailed;
    public ProgressBar progressBar;
    public DownPanel downPanel;
    #endregion

    #region main var 
    public PlayerController playerController;
    public CameraFollow mainCamera;
    #endregion

    // Инфо от левела
    private int curLevel = 0;
    private int countPartToWin = 0;
    private int countParts = 0;
    private int countRememberPart = 0;
    private int goldCount = 0;
    private Level levelModel;
    private Level nextlevelModel;
    private Level prevlevelModel;

    // Дополнительно
    public static bool calculateSuccessEnd = false;
    public static bool calculateFailingEnd = false;
    public static bool calculateFailingRevive = false;
    public static bool calculateParts = false;
    public static bool startNewLevel = false;
    public static bool calculateResidueVar = false;
    public static bool activateFirstTouch = false;
    public static bool calculateLackParts = false;
    public static bool setTextLevel = false;
    public static bool thirdWindow = false;
    public static bool sendEventBomb = false;
    public static bool sendEventBadLevel = false;
    public static bool sendEventSuccessLevel = false;
    private bool calculateRevive = false;
    private bool calculateReviveEnd = false;
    private bool firstTouch = true;

    public ParticleSystem psRevive;
    private int residue;
    private int prevResidue;

    private float timer = 1f;
    private float timerDelay = 0f;
    private float timerDelay2 = 0f;

    private bool gateCalculateResult = false;
    private float startPositionLevelPrefab = -3f;

    private int prevSavingTails;

    public Coloring coloring;

    public bool debug = false;

    void Awake()
    {
        Application.targetFrameRate = 60;

        int firstStart = PlayerPrefs.GetInt("FirstStart");
        if(firstStart != 1)
        {
            PlayerPrefs.SetInt("LevelNumber", 0);
            PlayerPrefs.SetInt("GoldCount", 0);

            PlayerPrefs.SetInt("SnakeSkin_0", 2);
            PlayerPrefs.SetInt("SnakeSkin_1", 0);
            PlayerPrefs.SetInt("SnakeSkin_2", 0);
            PlayerPrefs.SetInt("SnakeSkin_3", 0);
            PlayerPrefs.SetInt("SnakeSkin_4", 0);
            PlayerPrefs.SetInt("SnakeSkin_5", 0);
            PlayerPrefs.SetInt("SnakeSkin_6", 0);
            PlayerPrefs.SetInt("SnakeSkin_7", 0);
            PlayerPrefs.SetInt("SnakeSkin_8", 0);
            PlayerPrefs.SetInt("SnakeSkin_9", 0);

            PlayerPrefs.SetInt("FirstStart", 1);
        }

      
        StartGame();
        UpdateMenuView();
    }

 

    public void StartGame()
    {
      //  this.analyticsController.PushDesignEvent("cs:core:start_game");
        
        if(!debug)
        {
            curLevel = PlayerPrefs.GetInt("LevelNumber");
            countPartToWin = levelBase.levels[curLevel].countPartToWin;
            goldCount = PlayerPrefs.GetInt("GoldCount");

            Level newLevel = Instantiate(levelBase.levels[curLevel].prefab, new Vector3(0, 0, startPositionLevelPrefab), Quaternion.identity).GetComponent<Level>();
            newLevel.Initialization(playerController, countPartToWin);
            newLevel.ShowText();
            levelModel = newLevel;

            startPositionLevelPrefab += 42.7f;

            Level nextLevel = Instantiate(levelBase.levels[curLevel + 1].prefab, new Vector3(0, 0, 39.7f), Quaternion.identity).GetComponent<Level>();
            nextlevelModel = nextLevel;

            startPositionLevelPrefab += 42.7f;
        }
        else
        {
            countPartToWin = 15;
            levelModel = FindObjectOfType<Level>();
            levelModel.Initialization(playerController, countPartToWin);
            levelModel.ShowText();
        }

        coloring.InitializationOneScheme(levelBase.levels[curLevel].colorScheme, levelModel.textStart, levelModel.textFinish);
        coloring.SetColorSchemeForce();

        loadingScreen.gameObject.SetActive(true);
        loadingScreen.HideLoadScreen();

        progressBar.SetCountEmptyCell(countPartToWin);
        progressBar.AddCell(0, countPartToWin);

        levelComplete.gameObject.SetActive(true);
        levelComplete.HideWindow();

        levelFailed.gameObject.SetActive(true);
        levelFailed.HideWindow();

        downPanel.gameObject.SetActive(true);
    }

    void Update()
    {
        if(activateFirstTouch)
        {
            
            
            activateFirstTouch = false;
            firstTouch = true;
            levelModel.ShowText();
        }

        if(firstTouch && Input.GetMouseButtonDown(0) && !thirdWindow)
        {
            if (curLevelEventPassed != this.curLevel) {
//                this.analyticsController.PushDesignEvent($"cs:core:start_level:{this.curLevel + 1}");
                curLevelEventPassed = this.curLevel;
            }

            Vector3 screenPos = Input.mousePosition;
            float posH = screenPos.y / Screen.height;

            if(posH >= 0.00f) // 0.25f Если будет нижнее меню
            {
                firstTouch = false;
                playerController.FirstStart(Input.mousePosition, countPartToWin);
                playerController.playable = true;
                mainCamera.playable = true;
                downPanel.HideDownPanel();
            }

            levelModel.HideText();
        }

        // /////////////////////////////////////////////////////////////////////////////////

        if(calculateLackParts)
        {
            calculateLackParts = false;
            levelModel.finish.SetLackCounter();
        }

        if(calculateParts)
        {
            int currentSavingTails = playerController.currentSavingTails;

            if(calculateResidueVar)
            {
                calculateResidueVar = false;

                int collectedTails = playerController.GetComponent<SnakeTail>().positions.Count;
                
                residue = collectedTails;
                residue = Mathf.Clamp(residue, 0, countPartToWin);
                prevResidue = residue;
            }

            if(currentSavingTails != prevSavingTails)
            {
                prevSavingTails = currentSavingTails;
                levelModel.finish.UpdateCounter(currentSavingTails);
                residue--;
                residue = Mathf.Clamp(residue, 0, 1000);
            }
            
            if(residue < prevResidue)
            {
                prevResidue = residue;
                progressBar.SubtractCell(residue);
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////

        if(calculateSuccessEnd)
        {
            
            if(timer > 0)
            {
                levelModel.finish.DoorAnimator();
                timer -= Time.deltaTime;

                if(timer <= 0)
                {
                    levelComplete.ShowWindow();
                    levelModel.finish.ActivateCounter(false);

                    timerDelay = 3f;
                    gateCalculateResult = true;
                }
            }

            if(timerDelay > 0)
            { 
                if(gateCalculateResult)
                {
                    gateCalculateResult = false;
                    
                    upPanel.UpdateCoinBar(25, goldCount);

                    SaveData();
                    LoadingNewLevel();
                }

                timerDelay -= Time.deltaTime;

                if(timerDelay <= 0)
                {
                    levelComplete.HideWindow();

                    calculateSuccessEnd = false;
                    calculateParts = false;

                    prevSavingTails = 0;

                    CalculateEndLevel();
                }
            }
        }

        if(calculateFailingEnd)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;

                if(timer <= 0)
                {
//                    this.analyticsController.PushDesignEvent($"cs:core:bad_level:{this.curLevel + 1}");
                    
                    if(calculateFailingRevive)
                    {
                        levelFailed.ShowWindow(true);
                        timerDelay = 5.25f;
                        calculateFailingRevive = false;
                    }
                    else
                    {
                        levelFailed.ShowWindow(false);
                        timerDelay = 1.5f;
                        calculateParts = false;
                    }
                    
                }
            }

            if(timerDelay > 0)
            { 
                timerDelay -= Time.deltaTime;

                if(timerDelay <= 0)
                {
                    timerDelay = 0;
                    timerDelay2 = 0.25f;
                    loadingScreen.ShowLoadScreen();
                }
            }

            if(timerDelay2 > 0)
            {
                timerDelay2 -= Time.deltaTime;

                if(timerDelay2 <= 0)
                {
                    timerDelay2 = 0;
                    calculateFailingEnd = false;
                    SceneManager.LoadScene("MainScene");
                }
            }
        }

        if(calculateRevive)
        {
            if(timerDelay > 0)
            { 
                timerDelay -= Time.deltaTime;

                if(timerDelay <= 0)
                {
                    timerDelay = 0;
                    Time.timeScale = 0;
                    calculateRevive = false;

                    // Start Rewarded video
                    // ------- тут -------
                    // А пока реклама не интегрирована - сразу возвращаем игрока в игру
                    ReviveSuccess();
                }
            }
        }

        if(calculateReviveEnd)
        {
            if(timerDelay > 0)
            { 
                timerDelay -= Time.deltaTime;

                if(timerDelay <= 0)
                {
                    timerDelay = 0;
                    calculateReviveEnd = false;
                    
                    // Continue Game
                    activateFirstTouch = true;
                    mainCamera.speedMoveCamera = 1.5f;
                }
            }
        }

        if(startNewLevel)
        {
            startNewLevel = false;
            StartNewLevel();
        }
        
        
        // EVENT
        if (sendEventBomb) 
        {
//            this.analyticsController.PushDesignEvent($"cs:core:bad_level_bomb:{this.curLevel + 1}");
            sendEventBomb = false;
        }

        if (sendEventSuccessLevel) 
        {
           // this.analyticsController.PushDesignEvent($"cs:core:success_level:{this.curLevel + 1}");
            sendEventSuccessLevel = false;
        }
    }

    private void SaveData()
    {
        curLevel++;
        PlayerPrefs.SetInt("LevelNumber", curLevel);
        PlayerPrefs.SetInt("GoldCount", goldCount + 25);
    }

    private void LoadingNewLevel()
    {
        if(prevlevelModel != null)
        {
            Destroy(prevlevelModel.gameObject);
        }

        playerController.transform.GetComponent<SnakeTail>().DestroyParts(0, false);
        
        prevlevelModel = levelModel;
        levelModel = nextlevelModel;
        
        curLevel = PlayerPrefs.GetInt("LevelNumber");
        countPartToWin = levelBase.levels[curLevel].countPartToWin;
        goldCount = PlayerPrefs.GetInt("GoldCount");

        Level nextLevel = Instantiate(levelBase.levels[curLevel+1].prefab, new Vector3(0, 0, startPositionLevelPrefab), Quaternion.identity).GetComponent<Level>();
        nextlevelModel = nextLevel;

        levelModel.Initialization(playerController, countPartToWin);

        coloring.InitializationTwoScheme(levelBase.levels[curLevel - 1].colorScheme, levelBase.levels[curLevel].colorScheme, levelModel.textStart, levelModel.textFinish);
        coloring.SetColorSchemeLerp();

        startPositionLevelPrefab += 42.7f;
    }
    
    private void StartNewLevel()
    {
        progressBar.SetCountEmptyCell(countPartToWin);
        progressBar.AddCell(0, countPartToWin);
        UpdateMenuView();

        countParts = 0;
        residue = countPartToWin;
        prevResidue = countPartToWin + 1;

        timer = 1;
        timerDelay = 0;

        mainCamera.SetCameraToStartPosition(levelModel.transform.position.z);
        playerController.SetStartPosition(default, 0.65f ,levelModel.transform.position.z);
        downPanel.ShowDownPanel();
    }

    public void CalculateEndLevel()
    {
        prevlevelModel.finish.FinishAnimator();
    }

    public void UpdateMenuView()
    {
        upPanel.Initialization(goldCount, curLevel);
    }

    public void UpdateProgressBar(int newCountParts, bool addParts)
    {
        if(addParts)
        {
            countParts += newCountParts;
            progressBar.AddCell(countParts, countPartToWin);
        }
        else
        {
            countParts = newCountParts;
            progressBar.AddCell(countParts, countPartToWin);
        }
    }

    #region REVIVE
    public void SetRememberCountPart(int count)
    {
        countRememberPart = count;
    }
    // Press Button - (No, thanks)
    public void ReloadLevel()
    {
        timerDelay = 0;
        loadingScreen.ShowLoadScreen();
        timerDelay2 = 0.25f;
    }

    // Press Button - (Save, me!)
    public void GetRivive()
    {
        loadingScreen.ShowLoadScreen();
        calculateFailingEnd = false;
        timer = 1;
        timerDelay = 0.25f;
        timerDelay2 = 0;

        calculateRevive = true;
    }

    public void ReviveSuccess()
    {
        // Скрываем темный экран и выключаем окно проигрыша
        loadingScreen.HideLoadScreen();
        levelFailed.HideWindow();

        // Возвращаем TimeScale и устанавливаем задержку перед включением управления
        calculateReviveEnd = true;
        timerDelay = 0.25f;
        Time.timeScale = 1;

        // Скрытие объектов вокруг змейки (Чтобы при начале движения не наткнулась на старое препятствие)
        Collider[] hiden = Physics.OverlapSphere(playerController.transform.position, 5f);
        foreach(Collider other in hiden)
        {
            if(other.tag == "Untagged" || other.tag == "Assault")
            {
                other.gameObject.SetActive(false);
            }
        }

        // Возвращение разломаных кусочков на место, и сброс параметров движения змейки и восстановление ее подвижности
        SnakeTail st = playerController.GetComponent<SnakeTail>();
        st.ReviveSnake();
        Vector3 curPosSnake = playerController.transform.position;
        playerController.SetStartPosition(curPosSnake.x, curPosSnake.y, curPosSnake.z - 3);

        // Даем пользователю столько кусочков, сколько у него было.
        for(int i = 0; i < countRememberPart; i++)
        {
            st.AddPart();
        }

        // Добавляем визуальный эффект воскрешения
        psRevive.transform.position = playerController.transform.position;
        psRevive.Play();
        
//        this.analyticsController.PushDesignEvent($"cs:core:revive:{this.curLevel + 1}");
    }
    public void ReviveFailing()
    {
        // Пользователь не досмотрел рекламу
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }
    #endregion
}
