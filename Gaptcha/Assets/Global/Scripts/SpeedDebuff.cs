using UnityEngine;

public class SpeedDebuff : DebuffManager
{
    [SerializeField] float duration = 4f;
    [SerializeField] float speedMultiplier = 1.5f;

    float timer;
    bool isDebuffing;

    Player activePlayer;
    FlappyBirdPlayer flappyPlayer;

    float originalSpeed;
    float originalJumpPower;

    public override void OnDebuffEnter()
    {
        Debug.Log("[DEBUG] SpeedDebuff ENTER");

        activePlayer = FindObjectOfType<Player>();

        if (activePlayer == null) return;

        timer = 0f;
        isDebuffing = true;

        if (activePlayer is DodgePlayer)
        {
            originalSpeed = activePlayer.GetSpeed();
            activePlayer.SetSpeed(originalSpeed * speedMultiplier);
        }
        else if (activePlayer is PoopAvoidPlayer)
        {
            originalSpeed = activePlayer.GetSpeed();
            activePlayer.SetSpeed(originalSpeed * speedMultiplier);
        }
        else if (activePlayer is FlappyBirdPlayer)
        {
            flappyPlayer = activePlayer as FlappyBirdPlayer;
            if (flappyPlayer != null)
            {
                originalJumpPower = flappyPlayer.GetJumpPower();
                flappyPlayer.SetJumpPower(originalJumpPower * speedMultiplier);
            }
        }
    }

    public override void DebuffUpdate()
    {
        if (!isDebuffing) return;

        timer += Time.deltaTime;
        if (timer >= duration)
            OnDebuffExit();
    }

    public override void OnDebuffExit()
    {
        if (!isDebuffing) return;

        if (activePlayer is DodgePlayer || activePlayer is PoopAvoidPlayer)
        {
            activePlayer.SetSpeed(originalSpeed);
        }
        else if (activePlayer is FlappyBirdPlayer)
        {
            if (flappyPlayer != null)
                flappyPlayer.SetJumpPower(originalJumpPower);
        }

        isDebuffing = false;
    }
}
