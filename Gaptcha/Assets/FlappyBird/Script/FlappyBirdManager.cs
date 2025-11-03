using UnityEngine;

public class FlappyBirdManager : GameManager
{
    [SerializeField] PipeGenerator pipeGenerator;
    [SerializeField] FlappyBirdPlayer flappyBirdPlayer;

    protected override void SetGameKind()
    {
        gameKind = GameKind.FlappyBird;
    }
    public override void Refresh()
    {
        flappyBirdPlayer.Init();
        pipeGenerator.Init();
    }

    public override void GameStart()
    {

    }


}
