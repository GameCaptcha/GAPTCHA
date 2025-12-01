using UnityEngine;

public class PoopSpawner : UpdateBehaviour
{
    [SerializeField] Transform poopParent;
    [SerializeField] PoopFactory poopFactory;
    [SerializeField] private AfterImageDebuff _shadowDebuff;
    
    //public Poop poopPrefab;
    public float spawnY = 6f;
    public Vector2 xRange = new Vector2(-5.0f, 5.0f);
    public float spawnInterval = 0.8f;
    public float poopSpeed = 3f;

    float timer;

    private float currentObstacleMultiplier = 1.0f;


    public void Init()
    {
        timer = 0.0f;
        currentObstacleMultiplier = 1.0f;
    }

    public void Refresh()
    {
        poopFactory.Refresh();
        currentObstacleMultiplier = 1.0f;
    }

    public void SetObstacleSpeedMultiplier(float multiplier)
    {
        currentObstacleMultiplier = multiplier;

        Poop[] activePoops = poopParent.GetComponentsInChildren<Poop>();
        foreach (var poop in activePoops)
        {
            poop.SetSpeedMultiplier(multiplier);
        }
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        timer += Time.fixedDeltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        float x = Random.Range(xRange.x, xRange.y);
        Vector3 pos = new Vector3(x, spawnY, 0f);
        //Poop go = Instantiate(poopPrefab, poopParent);
        Poop go = poopFactory.UseObject();
        go.transform.parent = poopParent;
        go.transform.localPosition = pos;
        go.tag = "Poop";

        if (go != null) 
        {
            go.Init(poopSpeed, _shadowDebuff, poopFactory.Restore);
            go.SetSpeedMultiplier(currentObstacleMultiplier);
        }
    }
}
