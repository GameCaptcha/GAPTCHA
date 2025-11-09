using UnityEngine;
using UnityEngine.UI;

public class PoopAvoidManager : GameManager
{
    public PoopSpawner spawner;
    public Text timeText;
    public GameObject gameOverPanel;

    float surviveTime;
    bool isGameOver;

    protected override void SetGameKind()
    {
        gameKind = GameKind.PoopAvoid;
    }
    public override void Refresh()
    {
        spawner.enabled = true;
        spawner.Refresh();

        surviveTime = 0f;
        isGameOver = false;

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    public override void GameStart()
    {

    }

    void Update()
    {
        if (isGameOver) 
            return;

        surviveTime += Time.deltaTime;
        if (timeText)
        {
            timeText.text = $"TIME: {surviveTime:0.0}s";
        }
    }

    public void OnPlayerHit()
    {
        if (isGameOver) 
            return;

        isGameOver = true;

        if (spawner)
        {
            spawner.enabled = false;
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }

        GlobalGameManager.Instance.GameOver();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
