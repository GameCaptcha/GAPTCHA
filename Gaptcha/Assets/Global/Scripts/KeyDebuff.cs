using UnityEngine;
using System;

public class KeyDebuff : DebuffManager
{
    private bool _active = false;

   
    private int _mappedKey;

   
    public event Action<int> OnKeyChanged;

    public bool IsActive => _active;
    public int MappedKey => _mappedKey;

    public override void OnDebuffEnter()
    {
        _active = true;

        int[] candidates = { InputValue.LEFT, InputValue.RIGHT, InputValue.UP, InputValue.DOWN };
        int rand = UnityEngine.Random.Range(0, candidates.Length);

        _mappedKey = candidates[rand];

        Debug.Log("[KeyDebuff] New mapped key = " + _mappedKey);

       
        OnKeyChanged?.Invoke(_mappedKey);
    }

    public override void OnDebuffExit()
    {
        _active = false;
        OnKeyChanged?.Invoke(-1); 
    }

    public override void DebuffUpdate() { }
}
