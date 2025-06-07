using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D)), RequireComponent(typeof(SpriteRenderer))]
public class LightFollowSprite : MonoBehaviour
{
    
    [SerializeField] private Light2D _light2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (_light2D == null)
        {
            _light2D = GetComponent<Light2D>();
        }

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if(_light2D.lightType == Light2D.LightType.Sprite)
            _light2D.lightCookieSprite = _spriteRenderer.sprite;
    }

    private void Reset()
    {
        _light2D = GetComponent<Light2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _light2D.lightType = Light2D.LightType.Sprite;
        _light2D.lightCookieSprite = _spriteRenderer.sprite;
    }
}
