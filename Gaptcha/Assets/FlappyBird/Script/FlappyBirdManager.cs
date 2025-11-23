using UnityEngine;

public class FlappyBirdManager : GameManager
{
    [SerializeField] PipeGenerator pipeGenerator;
    [SerializeField] FlappyBirdPlayer flappyBirdPlayer;
    [SerializeField] public GameObject flappyUI;
    [SerializeField] KeyDebuff keyDebuff;
    [SerializeField] GlobalGameManager gm;

    public override void SetGameKind()
    {
        gameKind = GameKind.FlappyBird;
    }
    public override void Refresh()
    {

        flappyUI.SetActive(false);

        flappyBirdPlayer.Refresh();
        pipeGenerator.Init();

        if (keyDebuff != null)
            keyDebuff.OnDebuffExit();

        flappyUI.SetActive(true);


    }

    public override void GameOver()
    {
        flappyUI.SetActive(false);
    }

    void Update()
    {
     

        if (gm.GetNowGameKind() != GameKind.FlappyBird)
            return;

        flappyBirdPlayer.InputUpdate();
    }


}
