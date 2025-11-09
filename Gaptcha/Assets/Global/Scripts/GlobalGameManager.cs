using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalGameManager : MonoBehaviour
{
    [FormerlySerializedAs("ActionIndex")] public int actionIndex = 0;

    [SerializeField] Camera mainCamera;
    [SerializeField] VisualAgent visualAgent;


    [SerializeField] List<GameManager> gameManagerList = new List<GameManager>();
    GameManager nowGameManager = null;

    float gameChangeDelay = 8.0f;

    void Awake()
    {
        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            GlobalDatas.DebugLog("Init(): gameManagerList[i]=" + gameManagerList[i]);
            gameManagerList[i].gameObject.SetActive(false);
        }

        GameChange();
    }

    private void Start()
    {
        GlobalDatas.DebugLog("Start");
        GameStart();
    }

    public void GameChange()
    {
        int num = UnityEngine.Random.Range(0, gameManagerList.Count);
        GlobalDatas.DebugLog(num);

        if (nowGameManager == null)
        {
            nowGameManager = gameManagerList[num];
        }
        else
        {
            if (nowGameManager == gameManagerList[num])
            {
                if (gameManagerList.Count <= 1)
                    return;
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
            GlobalDatas.DebugLogError("not exist Game manager");
            return;
        }

        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            GlobalDatas.DebugLog("GameStart(): for: gameManagerList[i]=" + gameManagerList[i]);
            gameManagerList[i].gameObject.SetActive(false);
        }
        GlobalDatas.DebugLog("GameStart(): nowGameManager=" + nowGameManager);
        nowGameManager.gameObject.SetActive(true);

        nowGameManager.Refresh();
    }

    public void GameOver()
    {
        GlobalDatas.DebugLog("GameOver()");

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
            GlobalDatas.DebugLogError("PerformOnEpisodeBegin(): not exist nowGameManager");
        }
    }

}
