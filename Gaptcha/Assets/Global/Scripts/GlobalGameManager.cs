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
    float gameChangeDelay = 8.0f;

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

        if(elapsedTime >= gameChangeDelay)
        {
            GameChange();
            elapsedTime -= gameChangeDelay;
            GameStart();
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
