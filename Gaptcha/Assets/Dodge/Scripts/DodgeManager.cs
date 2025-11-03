using UnityEngine;

public class DodgeManager : GameManager
{
    [SerializeField] Player player;
    [SerializeField] BulletManager bulletManager;

    protected override void SetGameKind()
    {
        gameKind = GameKind.Dodge;
    }
    public override void Refresh()
    {
        bulletManager.Init();
        bulletManager.Refresh();
    }

    public override void GameStart()
    {

    }


    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
}
