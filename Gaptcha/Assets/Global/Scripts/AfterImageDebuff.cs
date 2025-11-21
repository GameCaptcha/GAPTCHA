using System;
using UnityEngine;

public class AfterImageDebuff : DebuffManager {
  private bool _isActive;

  public bool IsActive => _isActive;


  public event Action<bool> OnShadowToggle;
  
  public override void OnDebuffEnter() {
    _isActive = true;
    OnShadowToggle?.Invoke(true);
  }

  public override void OnDebuffExit() {
    _isActive = false;
    OnShadowToggle?.Invoke(false);
  }

  public override void DebuffUpdate() {

  }
}