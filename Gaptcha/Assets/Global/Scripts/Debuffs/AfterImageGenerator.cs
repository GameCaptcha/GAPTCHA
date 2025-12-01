using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageGenerator : UpdateBehaviour
{
  [SerializeField] private AfterImageDebuff _debuff;
  [SerializeField] SpriteRenderer _sprite;
  [SerializeField] float _coolDown = 0.05f;
  [SerializeField] float _fadeTime = 2f;
  [SerializeField] Color _shadowColor;

  private List<GameObject> _afterImages = new List<GameObject>();
  
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

    DestroyAfterImage();
  }
  
  void DestroyAfterImage() {
    
    foreach (GameObject afterImage in _afterImages) {
      if (afterImage != null) {
        Destroy(afterImage);
      }
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
    GameObject afterImage = new GameObject("Afterimage");
    Destroy(afterImage.gameObject, _fadeTime);
    _afterImages.Add(afterImage);
    
    afterImage.transform.position = transform.position;
    afterImage.transform.rotation = transform.rotation;
    afterImage.transform.localScale = transform.localScale;

    SpriteRenderer afterImageSprite = afterImage.AddComponent<SpriteRenderer>();
    afterImageSprite.sprite = _sprite.sprite;
    afterImageSprite.color = _shadowColor;
    
    afterImageSprite.flipX = _sprite.flipX;
    afterImageSprite.flipY = _sprite.flipY;
    afterImageSprite.sortingLayerID = _sprite.sortingLayerID;
    afterImageSprite.sortingOrder = _sprite.sortingOrder - 1;
    
    StartCoroutine(FadeEffect(afterImageSprite));
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
