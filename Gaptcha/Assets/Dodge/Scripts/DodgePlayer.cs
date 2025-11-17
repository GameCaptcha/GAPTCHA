using UnityEngine;

public class DodgePlayer : Player
{

    protected override void Init()
    {
        base.Init();
        speed = 5.5f;
        refreshPosition = Vector3.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            GlobalDatas.DebugLog(() => "Hit bullet");
            globalGameManager.GameOver();
        }
    }

    protected override void Move()
    {
        base.Move();

        Vector3 dir = new Vector3(movePosX, movePosY, 0f).normalized;

        transform.localPosition += dir * speed * Time.fixedDeltaTime;

        CheckPlayableBound();
    }
}
