using System.Collections;
using UnityEngine;

public class AfterImageGenerator : UpdateBehaviour
{
  [SerializeField] private AfterImageDebuff _debuff;
  [SerializeField] SpriteRenderer _sprite;
  [SerializeField] float _coolDown = 0.05f;
  [SerializeField] float _fadeTime = 0.3f;
  [SerializeField] Color _shadowColor;

  private float _timer;
  private bool _isActive;

  public AfterImageDebuff Debuff {
    get => _debuff;
    set => _debuff = value;
  }

  public bool IsActive {
    get { return _isActive; }
    set {
      _isActive = value;
      if(!value) _timer = 0;
    }
  }

  void Awake() {
    _sprite = GetComponent<SpriteRenderer>();
  }
  
  void OnEnable() {
    if (_debuff != null) {
      _debuff.OnImageDebuffToggle += HandleDebuffToggle;

      IsActive = _debuff.IsActive;
    }
    else {
      IsActive = false;
    }
  }
  
  void OnDisable() {
    if (_debuff != null) {
      _debuff.OnImageDebuffToggle -= HandleDebuffToggle;
    }
  }
  
  void HandleDebuffToggle(bool active) {
    IsActive = active;
  }
  
  public void SetDebuff(AfterImageDebuff debuff)
  {
    if (_debuff != null)
      _debuff.OnImageDebuffToggle -= HandleDebuffToggle;

    _debuff = debuff;

    if (_debuff != null)
    {
      _debuff.OnImageDebuffToggle += HandleDebuffToggle;
      
      IsActive = _debuff.IsActive;
    }
  }
  
  
  protected override void FUpdate()
  { 
    if (!_isActive || _sprite == null)
      return;

    _timer += Time.deltaTime;
    if (_timer >= _coolDown)
    {
      _timer -= _coolDown;
      SpawnAfterImage();
    }
  }

  void SpawnAfterImage() {
    GameObject shadow = new GameObject("Afterimage");
    Destroy(shadow.gameObject, _fadeTime);
    shadow.transform.position = transform.position;
    shadow.transform.rotation = transform.rotation;
    shadow.transform.localScale = transform.localScale;

    SpriteRenderer shadowSprite = shadow.AddComponent<SpriteRenderer>();
    shadowSprite.sprite = _sprite.sprite;
    shadowSprite.color = _shadowColor;
    
    shadowSprite.flipX = _sprite.flipX;
    shadowSprite.flipY = _sprite.flipY;
    shadowSprite.sortingLayerID = _sprite.sortingLayerID;
    shadowSprite.sortingOrder = _sprite.sortingOrder;

    StartCoroutine(FadeEffect(shadowSprite));
  }

  IEnumerator FadeEffect(SpriteRenderer shadow) {
    Color fadeColor = shadow.color;
    float timer = 0f;
    float clear = 1f;


    while (timer < _fadeTime) {
      timer += Time.fixedDeltaTime;
      clear -= 1 * (1 / _fadeTime * Time.fixedDeltaTime);

      fadeColor.a = clear;
      shadow.color = fadeColor;
      yield return new WaitForFixedUpdate();
    }
  }
}
