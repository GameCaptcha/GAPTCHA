using System;
using UnityEngine;

public class Bullet : SpriteBehaviour
{
    [SerializeField] AfterImageGenerator afterImageGenerator;

    Vector3 targetPosition;
    float speed;
    Vector2 normalizedTargetVector;

    float elapsedTime;
    float destroyTime = 3.5f;

    Action<Bullet> restoreAction = null;

    public void Init(Vector2 createPosition, Transform targetTransform, Action<Bullet> action, AfterImageDebuff afterImageDebuff)
    {
        elapsedTime = 0f;
        transform.localPosition = createPosition;

        targetPosition = targetTransform.localPosition;

        afterImageGenerator.SetDebuff(afterImageDebuff);
        normalizedTargetVector = (targetPosition - transform.localPosition).normalized;

        speed = 5.0f;

        restoreAction = action;
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        transform.localPosition += (Vector3)normalizedTargetVector * speed * Time.fixedDeltaTime;
        elapsedTime += Time.fixedDeltaTime;
        if (elapsedTime >= destroyTime)
        {
            //Destroy(gameObject);
            restoreAction?.Invoke(this);
        }
    }
}
