using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private const float Distance = 5f;

    public void Init()
    {
        _text.alpha = 1;
    }
    
    public void Show(double damage, float duration)
    {
        _text.text = damage.ToString("N0");
        Sequence seq = DOTween.Sequence();
        seq.Append(_text.DOFade(0, duration)).OnComplete(() => { DamageFloaterSpawner.Instance.HideDamage(this); });
        seq.Join(transform.DOMoveY(transform.position.y + Distance, duration).SetEase(Ease.Linear));
    }
}