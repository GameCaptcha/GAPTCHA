using UnityEngine;

public class TrainingAreaManager : UpdateBehaviour
{
    [SerializeField] Transform trainingAreaParent;
    [SerializeField] TrainingArea trainingAreaPrefab;

    public int X_MAX = 16;
    public int Y_MAX = 9;

    void Start()
    {
        for (int x = 0; x < X_MAX; ++x) 
        {
            for (int y = 0; y < Y_MAX; ++y) 
            {
                TrainingArea trainingArea = Instantiate(trainingAreaPrefab, trainingAreaParent);
                trainingArea.Init(x, y);
            }
        }
    }
}
