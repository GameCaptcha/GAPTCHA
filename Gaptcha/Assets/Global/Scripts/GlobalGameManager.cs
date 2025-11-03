using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager instance = null;

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


    [SerializeField] List<GameManager> gameManagerList = new List<GameManager>();
    GameManager nowGameManager = null;

    float elapsedTime;
    float elapsedTime_Total;
    float gameChangeDelay = 8.0f;
    int gameChangedCount = 0;

    void Init()
    {
        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            gameManagerList[i].gameObject.SetActive(false);
        }

        GameChange();
    }

    private void Start()
    {
        GameStart();
    }

    private void Update()
    {
        elapsedTime += Time.smoothDeltaTime;
        elapsedTime_Total += Time.smoothDeltaTime;
        GlobalDatas.SetAliveTime(elapsedTime_Total);

        if (elapsedTime >= gameChangeDelay)
        {
            GameChange();
            gameChangedCount += 1;
            elapsedTime -= gameChangeDelay;
            GameStart();

            GlobalDatas.SetScore(gameChangedCount);
        }

    }

    public void GameChange()
    {
        int num = UnityEngine.Random.Range(0, gameManagerList.Count);
        Debug.Log(num);

        if(nowGameManager == null)
        {
            nowGameManager = gameManagerList[num];
        }
        else
        {
            if(nowGameManager == gameManagerList[num])
            {
                GameChange();
            }
            else
            {
                nowGameManager = gameManagerList[num];
            }
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
            gameManagerList[i].gameObject.SetActive(false);
        }
        nowGameManager.gameObject.SetActive(true);

        nowGameManager.Refresh();
        nowGameManager.GameStart();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log(GlobalDatas.GetScore());
        Debug.Log(GlobalDatas.GetAliveTime());
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

}
