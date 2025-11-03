using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    protected GameKind gameKind;

    public abstract void GameStart();
    public abstract void Refresh();

    protected abstract void SetGameKind();
    public GameKind GetGameKind()
    {
        return gameKind;
    }
}
