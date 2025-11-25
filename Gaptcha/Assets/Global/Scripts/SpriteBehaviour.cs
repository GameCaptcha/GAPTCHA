using System.Collections.Generic;
using UnityEngine;

public class SpriteBehaviour : UpdateBehaviour
{
    [SerializeField] List<SpriteRenderer> spriteRendererList = new List<SpriteRenderer>();

    public void SetMaterial(Material material)
    {
        for (int i = 0; i < spriteRendererList.Count; ++i)
        {
            spriteRendererList[i].material = material;
        }
    }
}
