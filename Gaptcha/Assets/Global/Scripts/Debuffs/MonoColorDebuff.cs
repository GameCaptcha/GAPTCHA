using UnityEngine;

public class MonoColorDebuff : ShaderDebuff
{
    [SerializeField] protected Material monoColorMaterial;


    public override void OnDebuffEnter()
    {
        base.OnDebuffEnter();

        RefreshMaterial(monoColorMaterial);
    }

    public override void OnDebuffExit()
    {
        base.OnDebuffExit();

    }

    public override void DebuffUpdate()
    {
        base.DebuffUpdate();

    }
}
