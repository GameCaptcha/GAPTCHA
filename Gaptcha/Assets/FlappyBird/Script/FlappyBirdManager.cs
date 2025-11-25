using UnityEngine;

public class FlappyBirdManager : GameManager
{
    [SerializeField] PipeGenerator pipeGenerator;
    [SerializeField] FlappyBirdPlayer flappyBirdPlayer;

    void Awake()
    {
        SetGameKind();
    }

    protected override void SetGameKind()
    {
        gameKind = GameKind.FlappyBird;
    }
    public override void Refresh()
    {
        flappyBirdPlayer.Refresh();
        pipeGenerator.Init();
    }

    public override void GameOver()
    {

    }

}
