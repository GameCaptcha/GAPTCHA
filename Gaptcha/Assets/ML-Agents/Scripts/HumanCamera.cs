using UnityEngine;

public class HumanCamera : UpdateBehaviour
{
    [SerializeField] Camera humanCamera;

    public void Init(int xindex, int yindex)
    {
        float x = 1.0f / 16;
        float y = 1.0f / 9;

        humanCamera.rect = new Rect(x * xindex, y * yindex, x, y);
    }    
}
