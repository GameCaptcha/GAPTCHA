using System.Collections.Generic;
using UnityEngine;

public class BulletManager : UpdateBehaviour
{
    [SerializeField] DodgeManager dodgeManager;

    //[SerializeField] Bullet bulletPrefab;
    [SerializeField] BulletFactory bulletFactory;
    [SerializeField] Transform bulletParent;
    
    [SerializeField] AfterImageDebuff _shadowDebuff;

    float elapsedTime;
    float delayTime;
    int makeCount;

    float createSize = 6.5f;

    private float currentObstacleMultiplier = 1.0f;

    public void Init()
    {
        elapsedTime = 0.0f;
        delayTime = 2.0f;
        makeCount = 6;
        currentObstacleMultiplier = 1.0f;
    }

    public void Refresh()
    {
        bulletFactory.Refresh();
        currentObstacleMultiplier = 1.0f;
    }

    public void SetObstacleSpeedMultiplier(float multiplier)
    {
        currentObstacleMultiplier = multiplier;

        Bullet[] activeBullets = bulletParent.GetComponentsInChildren<Bullet>();
        foreach (var bullet in activeBullets)
        {
            bullet.SetSpeedMultiplier(multiplier);
        }
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        elapsedTime += Time.fixedDeltaTime;

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

            Bullet bullet = bulletFactory.UseObject();
            bullet.transform.parent = bulletParent;
            bullet.Init(createPosition, dodgeManager.GetPlayerTransform(), bulletFactory.Restore, _shadowDebuff);

            bullet.SetSpeedMultiplier(currentObstacleMultiplier);
        }
        makeCount += 1;
    }
}
