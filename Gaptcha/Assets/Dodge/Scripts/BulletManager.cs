using System.Collections.Generic;
using UnityEngine;

public class BulletManager : UpdateBehaviour
{
    [SerializeField] DodgeManager dodgeManager;

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletParent;

    float elapsedTime;
    float delayTime;
    int makeCount;

    float createSize = 6.5f;

    public void Init()
    {
        elapsedTime = 0.0f;
        delayTime = 2.0f;
        makeCount = 6;
    }

    public void Refresh()
    {
        foreach (Transform child in bulletParent)
        {
            Destroy(child.gameObject);
        }
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        elapsedTime += Time.smoothDeltaTime;

        if (elapsedTime >= delayTime) 
        {
            MakeBullet();
            elapsedTime -= delayTime;
        }
    }

    void MakeBullet()
    {
        for (int i = 0; i < makeCount; ++i) 
        {
            Vector2 createPosition = new Vector2();
            int rand1 = Random.Range(0, 2);
            int rand2 = Random.Range(0, 2);
            rand2 = rand2 == 0 ? -1 : 1;
            if (rand1 == 0)
            {
                createPosition.x = rand2 * createSize;
                createPosition.y = Random.Range(-createSize, createSize);
            }
            else
            {
                createPosition.x = Random.Range(-createSize, createSize);
                createPosition.y = rand2 * createSize;
            }

            Bullet bullet = Instantiate(bulletPrefab, bulletParent);
            bullet.Init(createPosition, dodgeManager.GetPlayerTransform());
        }
        makeCount += 1;
    }
}
