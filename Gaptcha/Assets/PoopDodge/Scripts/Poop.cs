using System;
using UnityEngine;

public class Poop : SpriteBehaviour
{
    [SerializeField] AfterImageGenerator afterImageGenerator;

    float baseFallSpeed = 3f;
    float currentFallSpeed;
    float destroyY = -6f;
    Action<Poop> restoreAction = null;

    public void Init(float speed, AfterImageDebuff afterImageDebuff, Action<Poop> action)
    {
        baseFallSpeed = speed;
        afterImageGenerator.SetDebuff(afterImageDebuff);
        restoreAction = action;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        currentFallSpeed = baseFallSpeed * multiplier;
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += Vector3.down * currentFallSpeed * Time.fixedDeltaTime;
        if (transform.localPosition.y < destroyY)
        {
            restoreAction?.Invoke(this);
        }

    }
}
