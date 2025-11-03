using UnityEngine;

public class Bullet : MonoBehaviour
{

    Vector3 targetPosition;
    float speed;
    Vector2 normalizedTargetVector;

    float elapsedTime;
    float destroyTime = 3.5f;

    public void Init(Vector2 createPosition, Transform targetTransform)
    {
        elapsedTime = 0f;
        transform.localPosition = createPosition;

        targetPosition = targetTransform.localPosition;

        normalizedTargetVector = (targetPosition - transform.localPosition).normalized;

        speed = 5.0f;
    }

    private void Update()
    {
        transform.localPosition += (Vector3)normalizedTargetVector * speed * Time.smoothDeltaTime;
        elapsedTime += Time.smoothDeltaTime;
        if (elapsedTime >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
