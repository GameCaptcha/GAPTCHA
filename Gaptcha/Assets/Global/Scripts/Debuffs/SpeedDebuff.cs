using UnityEngine;

public class SpeedDebuff : DebuffManager
{
    [SerializeField] float duration = 4f;
    [SerializeField] float speedMultiplier = 1.5f;

    float timer;
    bool isDebuffing;

    Player activePlayer;


    float originalSpeed;


    public override void OnDebuffEnter()
    {
        GlobalDatas.DebugLog("[DEBUG] SpeedDebuff ENTER");

        if (isDebuffing)
            return;

        activePlayer = activePlayer = this.GetComponentInParent<TrainingArea>().GetComponentInChildren<Player>();


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
        isDebuffing = false;
    }
}