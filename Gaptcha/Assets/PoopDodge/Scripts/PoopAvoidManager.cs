using UnityEngine;
using UnityEngine.UI;

public class PoopAvoidManager : GameManager
{
    [SerializeField] GlobalGameManager globalGameManager;
    public PoopSpawner spawner;
    public Text timeText;

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

        globalGameManager.GameOver();
    }
}
