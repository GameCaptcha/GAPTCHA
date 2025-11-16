using UnityEngine;

public class DodgeManager : GameManager
{
    [SerializeField] BulletManager bulletManager;

    protected override void SetGameKind()
    {
        gameKind = GameKind.Dodge;
    }
    public override void Refresh()
    {
        bulletManager.Init();
        bulletManager.Refresh();
        player.Refresh();
    }

    public override void GameOver()
    {

    }


    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
}
