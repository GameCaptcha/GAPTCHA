using UnityEngine;

public class SpeedDebuff : DebuffManager
{
    [SerializeField] GlobalGameManager globalGameManager;

    float duration;
    float timer;
    bool isDebuffing;

    Player activePlayer;
    float originalPlayerSpeed;

    float playerMultiplier;
    float obstacleMultiplier;

    public override void OnDebuffEnter()
    {
        if (isDebuffing) return;

        if (globalGameManager == null)
            globalGameManager = GetComponentInParent<GlobalGameManager>();

        var trainingArea = this.GetComponentInParent<TrainingArea>();
        activePlayer = trainingArea.GetComponentInChildren<Player>();

        if (activePlayer == null) return;

        timer = 0f;
        isDebuffing = true;

        float maxDuration = 8.0f;
        if (globalGameManager != null)
        {
            maxDuration = globalGameManager.GetGameChangeDelay();
        }

        duration = Random.Range(1.0f, maxDuration);

        playerMultiplier = Random.Range(2.0f, 3.0f);
        obstacleMultiplier = Random.Range(1.5f, 2.0f);

        originalPlayerSpeed = activePlayer.GetSpeed();
        activePlayer.SetSpeed(originalPlayerSpeed * playerMultiplier);

        if (activePlayer is DodgePlayer)
        {
            BulletManager bulletManager = trainingArea.GetComponentInChildren<BulletManager>();
            if (bulletManager != null)
            {
                bulletManager.SetObstacleSpeedMultiplier(obstacleMultiplier);
            }
        }
        else if (activePlayer is PoopAvoidPlayer)
        {
            PoopSpawner poopSpawner = trainingArea.GetComponentInChildren<PoopSpawner>();
            if (poopSpawner != null)
            {
                poopSpawner.SetObstacleSpeedMultiplier(obstacleMultiplier);
            }
        }
    }

    public override void DebuffUpdate()
    {
        if (!isDebuffing) return;

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            OnDebuffExit();
        }
    }

    public override void OnDebuffExit()
    {
        if (!isDebuffing) return;

        if (activePlayer != null)
        {
            activePlayer.SetSpeed(originalPlayerSpeed);
        }

        var trainingArea = this.GetComponentInParent<TrainingArea>();

        if (activePlayer is DodgePlayer)
        {
            BulletManager bulletManager = trainingArea.GetComponentInChildren<BulletManager>();
            if (bulletManager != null)
            {
                bulletManager.SetObstacleSpeedMultiplier(1.0f);
            }
        }
        else if (activePlayer is PoopAvoidPlayer)
        {
            PoopSpawner poopSpawner = trainingArea.GetComponentInChildren<PoopSpawner>();
            if (poopSpawner != null)
            {
                poopSpawner.SetObstacleSpeedMultiplier(1.0f);
            }
        }

        isDebuffing = false;
    }
}