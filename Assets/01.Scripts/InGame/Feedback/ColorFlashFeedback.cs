using System.Collections;
using UnityEngine;

public class ColorFlashFeedback : MonoBehaviour, IFeedback
{
    private Coroutine _coroutine;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _flashDuration;
    [SerializeField] private Color _flashColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Auto) return;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        _coroutine = StartCoroutine(ChangeColor_Coroutine());
    }
    
    private IEnumerator ChangeColor_Coroutine()
    {
        _spriteRenderer.color = _flashColor;

        yield return new WaitForSeconds(_flashDuration);
        
        _spriteRenderer.color = Color.white;
    }
}
