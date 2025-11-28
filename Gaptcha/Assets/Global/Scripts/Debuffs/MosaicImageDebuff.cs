using UnityEngine;

public class MosaicImageDebuff : ShaderDebuff
{
    [SerializeField] protected Material mosaicImageMaterial;


    public override void OnDebuffEnter()
    {
        base.OnDebuffEnter();

        RefreshMaterial(mosaicImageMaterial);
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
