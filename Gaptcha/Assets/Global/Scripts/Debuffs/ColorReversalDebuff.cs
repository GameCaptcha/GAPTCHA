using UnityEngine;

public class ColorReversalDebuff : ShaderDebuff
{
    [SerializeField] protected Material colorReversalMaterial;


    public override void OnDebuffEnter()
    {
        base.OnDebuffEnter();

        RefreshMaterial(colorReversalMaterial);
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
