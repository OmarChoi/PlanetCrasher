using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private const float Distance = 5f;

    private Sequence _sequence;
    private float _sequenceDuration;
    private float _moveProgress;
    private Vector3 _startPosition;
    private bool _isDespawned;

    public void Play(double damage, float duration)
    {
        _isDespawned = false;
        _moveProgress = 0f;
        _startPosition = transform.position;

        _text.alpha = 1f;
        _text.text = damage.ToFormattedString();

        EnsureSequence(duration);
        _sequence.Restart();
    }

    private void EnsureSequence(float duration)
    {
        if (_sequence != null && Mathf.Approximately(_sequenceDuration, duration)) return;

        _sequence?.Kill();
        _sequenceDuration = duration;

        _sequence = DOTween.Sequence()
                           .SetAutoKill(false)
                           .Pause();

        _sequence.Append(_text.DOFade(0f, duration));
        _sequence.Join(DOTween.To(GetMoveProgress, SetMoveProgress, 1f, duration).SetEase(Ease.Linear));
        _sequence.OnComplete(Despawn);
    }

    private float GetMoveProgress()
    {
        return _moveProgress;
    }

    private void SetMoveProgress(float progress)
    {
        _moveProgress = progress;
        transform.position = _startPosition + Vector3.up * (Distance * progress);
    }

    private void Despawn()
    {
        if (_isDespawned) return;
        _isDespawned = true;
        DamageFloaterSpawner.Instance.HideDamage(this);
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}
