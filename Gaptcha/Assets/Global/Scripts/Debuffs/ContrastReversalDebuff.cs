using UnityEngine;

public class ContrastReversalDebuff : ShaderDebuff
{
    [SerializeField] protected Material constrastReversalMaterial;


    public override void OnDebuffEnter()
    {
        base.OnDebuffEnter();

        RefreshMaterial(constrastReversalMaterial);
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
