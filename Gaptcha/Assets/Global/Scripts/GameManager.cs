using UnityEngine;

public abstract class GameManager : UpdateBehaviour
{
    public Player player;
    protected GameKind gameKind;

    public abstract void Refresh();
    public abstract void GameOver();

    public abstract void SetGameKind();
    public GameKind GetGameKind()
    {
        return gameKind;
    }
}
