using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager instance = null;
    
    [FormerlySerializedAs("ActionIndex")] public int actionIndex = 0;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Init();
    }
    public static GlobalGameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    [SerializeField] Camera mainCamera;
    [SerializeField] VisualAgent visualAgent;


    [SerializeField] List<GameManager> gameManagerList = new List<GameManager>();
    GameManager nowGameManager = null;

    float elapsedTime;
    float gameChangeDelay = 8.0f;

    void Init()
    {
        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            Debug.Log("Init(): gameManagerList[i]=" + gameManagerList[i]);
            gameManagerList[i].gameObject.SetActive(false);
        }

        GameChange();
    }

    private void Start()
    {
        Debug.Log("Start");
        GameStart();
    }

    private void Update()
    {
        elapsedTime += Time.smoothDeltaTime;

        // if(elapsedTime >= gameChangeDelay)
        // {
        //     GameChange();
        //     elapsedTime -= gameChangeDelay;
        //     GameStart();
        // }

    }

    public void GameChange()
    {
        // 무한 재귀 안 되도록 시간 최적화 요청
        int num = UnityEngine.Random.Range(0, gameManagerList.Count);
        if (VisualAgent.DEBUG_PRINT)
            Debug.Log(num);

        if(nowGameManager == null)
        {
            nowGameManager = gameManagerList[num];
        }
        else
        {
            // if(nowGameManager == gameManagerList[num])
            // {
            //     GameChange();
            // }
            // else
            // {
                nowGameManager = gameManagerList[num];
            // }
        }
    }

    void GameStart()
    {
        if(nowGameManager == null)
        {
            Debug.LogError("not exist Game manager");
            return;
        }

        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            Debug.Log("GameStart(): for: gameManagerList[i]=" + gameManagerList[i]);
            gameManagerList[i].gameObject.SetActive(false);
        }
        Debug.Log("GameStart(): nowGameManager=" + nowGameManager);
        nowGameManager.gameObject.SetActive(true);

        nowGameManager.Refresh();
        nowGameManager.GameStart();
    }

    //public bool over = false;
    public void GameOver()
    {
        if (VisualAgent.DEBUG_PRINT)
            Debug.Log("GameOver()");
        // Time.timeScale = 0;
        visualAgent.OnEndEpisode();
    }

    public GameKind GetRandomGameKind()
    {
        int count = UnityEngine.Random.Range(0, gameManagerList.Count);

        return gameManagerList[count].GetGameKind();
    }

    public GameKind GetNowGameKind()
    {
        return nowGameManager.GetGameKind();
    }

    public void PerformOnEpisodeBegin()
    {
        // Init();
        if (nowGameManager)
        {
            nowGameManager.Refresh();
        }
        else
        {
            Debug.LogError("PerformOnEpisodeBegin(): not exist nowGameManager");
        }
    }

}
