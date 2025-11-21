using UnityEngine;

public abstract class DebuffManager : UpdateBehaviour
{
    public abstract void DebuffUpdate();
    public abstract void OnDebuffEnter();
    public abstract void OnDebuffExit();
}
