using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private const float Distance = 5f;
    private Sequence _sequence;
    private bool _isDespawned;

    public void Init()
    {
        _sequence?.Kill();
        _isDespawned = false;
        _text.alpha = 1;
    }

    public void Show(double damage, float duration)
    {
        _text.text = damage.ToFormattedString();
        _sequence = DOTween.Sequence();
        _sequence.Append(_text.DOFade(0, duration));
        _sequence.Join(transform.DOMoveY(transform.position.y + Distance, duration).SetEase(Ease.Linear));
        _sequence.OnComplete(Despawn);
    }

    private void Despawn()
    {
        if (_isDespawned) return;
        _isDespawned = true;
        DamageFloaterSpawner.Instance.HideDamage(this);
    }
}