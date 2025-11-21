using System;
using UnityEngine;

public class AfterImageDebuff : DebuffManager {
  private bool _isActive;

  public bool IsActive => _isActive;


  public event Action<bool> OnImageDebuffToggle;
  
  public override void OnDebuffEnter() {
    _isActive = true;
    OnImageDebuffToggle?.Invoke(true);
  }

  public override void OnDebuffExit() {
    _isActive = false;
    OnImageDebuffToggle?.Invoke(false);
  }

  public override void DebuffUpdate() {

  }
}