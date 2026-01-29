using UnityEngine;
using DG.Tweening;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private float _targetScale;
    [SerializeField] private float _targetDuration;

    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.AutoClick) return;
        transform.DOKill(true);
        transform.DOScale(_targetScale, _targetDuration)
                 .OnComplete(() => { transform.localScale = Vector3.one; })
                 .SetEase(Ease.Linear);
    }
}
