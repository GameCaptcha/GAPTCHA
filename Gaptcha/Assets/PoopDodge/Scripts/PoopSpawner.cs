using UnityEngine;

public class PoopSpawner : UpdateBehaviour
{
    [SerializeField] Transform poopParent;
    [SerializeField] private AfterImageDebuff _shadowDebuff;
    
    public Poop poopPrefab;
    public float spawnY = 6f;
    public Vector2 xRange = new Vector2(-5.0f, 5.0f);
    public float spawnInterval = 0.8f;
    public float poopSpeed = 3f;

    float timer;

    public void Init()
    {
        timer = 0.0f;
    }

    public void Refresh()
    {
        foreach (Transform child in poopParent)
        {
            Destroy(child.gameObject);
        }
    }

    override protected void FUpdate()
    {
        base.FUpdate();
        timer += Time.deltaTime;
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
        Poop go = Instantiate(poopPrefab, poopParent);
        go.transform.localPosition = pos;
        go.tag = "Poop";

        if (go != null) {
            go.fallSpeed = poopSpeed;
            go.GetComponentInChildren<AfterImageGenerator>().SetDebuff(_shadowDebuff);
        }
    }
}
