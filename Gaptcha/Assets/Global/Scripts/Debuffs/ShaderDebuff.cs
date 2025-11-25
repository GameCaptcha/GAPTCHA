using System.Collections.Generic;
using UnityEngine;

public class ShaderDebuff : DebuffManager
{
    [SerializeField] protected Material defaultMaterial;

    protected List<SpriteBehaviour> spriteBehaviourList = new List<SpriteBehaviour>();

    public void Init(List<SpriteBehaviour> spriteList)
    {
        spriteBehaviourList = spriteList;
    }

    public override void OnDebuffEnter()
    {

    }

    public override void OnDebuffExit()
    {
        RefreshMaterial(defaultMaterial);
    }

    public override void DebuffUpdate()
    {

    }

    protected void RefreshMaterial(Material material)
    {
        for (int i = 0; i < spriteBehaviourList.Count; ++i)
        {
            spriteBehaviourList[i].SetMaterial(material);
        }
    }
}
