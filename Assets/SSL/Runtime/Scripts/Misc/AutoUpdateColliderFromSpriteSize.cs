using UnityEngine;

[ExecuteAlways]
public class AutoUpdateColliderFromSpriteSize : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _boxCollider.size = _spriteRenderer.size;
    }
}