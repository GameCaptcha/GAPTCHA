using System.Collections.Generic;
using UnityEngine;

public class KeyDebuff : DebuffManager
{
    [SerializeField] GlobalGameManager globalGameManager;
    private int targetKeyIndex = -1;
    private bool isDebuffActive = false;

    public event System.Action<int> OnKeyChanged;

    public override void OnDebuffEnter()
    {
        if (globalGameManager.GetNowGameKind() != GameKind.FlappyBird)
        {
            isDebuffActive = false;
            return;
        }
        

        List<int> candidateValues = new List<int>
        {
            InputValue.LEFT,
            InputValue.RIGHT,
            InputValue.UP,
            InputValue.DOWN
        };

        int randomValue = candidateValues[Random.Range(0, candidateValues.Count)];
        targetKeyIndex = GlobalDatas.ConvertInputValueToIndex(randomValue);

     

        isDebuffActive = true;

        GlobalDatas.DebugLog(() => "KeyDebuff Enter: Selected Key Value " + randomValue);
    }

    public override void DebuffUpdate()
    {

        if (!isDebuffActive) return;

        if (globalGameManager.GetNowGameKind() == GameKind.FlappyBird)
        {
            if (globalGameManager.currentJumpActionIndex != targetKeyIndex)
            {
                globalGameManager.currentJumpActionIndex = targetKeyIndex;

                int keyValue = GlobalDatas.ConvertInputIndexeToValue(targetKeyIndex);
                OnKeyChanged?.Invoke(keyValue);
            }
        }
        else
        {
            int spaceIndex = GlobalDatas.ConvertInputValueToIndex(InputValue.SPACE);
            if (globalGameManager.currentJumpActionIndex != spaceIndex)
            {
                globalGameManager.ResetJumpKey();
                OnKeyChanged?.Invoke(InputValue.SPACE);
            }
        }
    }

    public override void OnDebuffExit()
    {
        globalGameManager.ResetJumpKey();

        OnKeyChanged?.Invoke(InputValue.SPACE);

        isDebuffActive = false;
    }
}