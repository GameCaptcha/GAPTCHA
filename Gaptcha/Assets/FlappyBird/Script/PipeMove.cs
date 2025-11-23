using System;
using UnityEngine;

public class PipeMove : UpdateBehaviour
{
    
    [SerializeField] private float _movSpeed; // 파이프 이동속도

    Action<PipeMove> restoreAction = null;
    float elapsedTime = 0;
    bool isActive = false;

    public void Init(float time, Action<PipeMove> action)
    {
        restoreAction = action;
        elapsedTime = time;
        isActive = true;
    }    

    override protected void FUpdate()
    { 
        base.FUpdate();
        transform.localPosition += Vector3.left * (_movSpeed * Time.fixedDeltaTime);

        elapsedTime -= Time.fixedDeltaTime;
        if (elapsedTime <= 0 && isActive == true) 
        {
            restoreAction?.Invoke(this);
            isActive = false;
        }
    }
}
