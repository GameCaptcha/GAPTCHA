using UnityEngine;

public class TrainingArea : MonoBehaviour
{
    [SerializeField] HumanCamera humanCamera;

    public void Init(int xindex, int yindex)
    {
        transform.localPosition = new Vector3(xindex * 20, yindex * 20, 0);
        humanCamera.Init(xindex, yindex);
    }
}
