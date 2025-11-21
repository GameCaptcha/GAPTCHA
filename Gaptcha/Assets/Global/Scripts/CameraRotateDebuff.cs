using UnityEngine;

public class CameraRotateDebuff : DebuffManager {

  [SerializeField] Camera[] _targetCameras;

  [SerializeField] private float _tiltAngle;
  [SerializeField] private float _halfPhase;
  
  private float tilt;
  float _timer;
  private int _type;

  public override void OnDebuffEnter() {
    _timer = 0f;
    _type = Random.Range(0, 2);
    
    tilt = (_type == 0) ? _tiltAngle : -_tiltAngle;
  }

  public override void OnDebuffExit() {
    foreach (Camera cam in _targetCameras) {
      if (cam == null) continue;

      cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
  }
  
  public override void DebuffUpdate() {
    _timer += Time.deltaTime;
    float rotZ = 0f;
    
    if (_timer <= _halfPhase) {
      float progress = _timer / _halfPhase;
      rotZ = Mathf.Lerp(0f, tilt, progress);
    }
    else if (_timer <= _halfPhase * 2f) {
      float progress = (_timer - _halfPhase) / _halfPhase;
      rotZ = Mathf.Lerp(tilt, 0f, progress);
    }
    else {
      rotZ = 0f;
    }

    Quaternion targetRot = Quaternion.Euler(0f, 0f, rotZ);

    foreach (Camera cam in _targetCameras) {
      if (cam == null) continue;

      cam.transform.rotation = targetRot;
    }
  }
}
