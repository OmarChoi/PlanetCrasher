using UnityEngine;
using DG.Tweening;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private float _targetScale;
    [SerializeField] private float _targetDuration;

    private Tween _scaleTween;
    private TweenCallback _resetScaleCallback;

    private void Awake()
    {
        _resetScaleCallback = ResetScale;
        _scaleTween = transform.DOScale(_targetScale, _targetDuration)
                               .SetEase(Ease.Linear)
                               .OnComplete(_resetScaleCallback)
                               .SetAutoKill(false)
                               .Pause();
    }

    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.AutoClick) return;
        _scaleTween.Restart();
    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        _scaleTween?.Kill();
    }
}
