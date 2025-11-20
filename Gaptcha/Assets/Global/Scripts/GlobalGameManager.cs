using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalGameManager : UpdateBehaviour
{
    [FormerlySerializedAs("ActionIndex")] public int actionIndex = 0;

    [SerializeField] Camera mainCamera;
    [SerializeField] VisualAgent visualAgent;

    public GameObject gamesParent;
    [SerializeField] List<GameManager> gameManagerList = new List<GameManager>();
    GameManager nowGameManager = null;

    [SerializeField] float gameChangeDelay = 8.0f;
    float elapsedChangeTime = 0.0f;

    void Awake()
    {
        if (gamesParent != null)
        {
            int childCount = gamesParent.transform.childCount;
            // Disable all children from the parent
            // so even modules not added to gameManagerList stay inactive.
            // If we skip this, a prefab that is active in the parent
            // but missing from the list can keep running and create hard bugs.
            for (int i = 0; i < childCount; ++i)
            {
                Transform child = gamesParent.transform.GetChild(i);
                child.gameObject.SetActive(false);
                GlobalDatas.DebugLog("Awake(): deactivate child '" + child.gameObject.name + "'");
            }
        }
        else
        {
            GlobalDatas.DebugLogError("Awake(): gamesParent is not assigned");
        }

        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            // GlobalDatas.DebugLog("Init(): gameManagerList[i]=" + gameManagerList[i]);
            // gameManagerList[i].gameObject.SetActive(false);
            GlobalDatas.DebugLog("Awake(): deactivate gameManagerList[" + i + "]=" + gameManagerList[i]);
            gameManagerList[i].gameObject.SetActive(false);
        }

        GameChange();
    }

    // private void Start()
    // {
    //     GlobalDatas.DebugLog("Start");
    //     GameStart();
    // }
    protected override void FUpdate()
    {
        base.FUpdate();

        elapsedChangeTime += Time.fixedDeltaTime;
        if (elapsedChangeTime >= gameChangeDelay)
        {
            elapsedChangeTime -= gameChangeDelay;
            GameChange();
        }
    }

    public void GameChange(bool allowSame = false)
    {
        int count = gameManagerList.Count;
        if (count == 0)
        {
            GlobalDatas.DebugLogError("GameChange(): gameManagerList is empty");
            nowGameManager = null;
            return;
        }

        int selectedIndex;

        if (nowGameManager == null)
        {
            selectedIndex = UnityEngine.Random.Range(0, count);
        }
        else if (count == 1)
        {
            selectedIndex = 0;
        }
        else if (allowSame)
        {
            selectedIndex = UnityEngine.Random.Range(0, count);
        }
        else
        {
            int currentIndex = gameManagerList.IndexOf(nowGameManager);
            if (currentIndex < 0)
            {
                GlobalDatas.DebugLogError("GameChange(): not found nowGameManager in gameManagerList");
                selectedIndex = UnityEngine.Random.Range(0, count);
            }
            else
            {
                int nextIndex = UnityEngine.Random.Range(0, count - 1);
                if (nextIndex >= currentIndex)
                {
                    nextIndex++;
                }
                selectedIndex = nextIndex;
            }
        }

        if (nowGameManager)
        {
            nowGameManager.gameObject.SetActive(false);
        }
        nowGameManager = gameManagerList[selectedIndex];
        nowGameManager.gameObject.SetActive(true);
        GlobalDatas.DebugLog("GameChange(): nowGameManager=" + nowGameManager + ", index=" + selectedIndex);
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

        for (int i = 0; i < gameManagerList.Count; ++i)
        {
            gameManagerList[i].GameOver();
        }

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
            elapsedChangeTime = 0.0f;
        }
        else
        {
            GlobalDatas.DebugLogError("PerformOnEpisodeBegin(): not exist nowGameManager");
        }
    }

}
