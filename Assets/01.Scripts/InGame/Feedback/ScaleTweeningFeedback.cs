using UnityEngine;
using DG.Tweening;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    private ClickTarget _owner;
    [SerializeField] private float _targetScale;
    [SerializeField] private float _targetDuration;

    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }
    
    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.AutoClick) return;
        _owner.transform.DOKill(true);
        _owner.transform.DOScale(_targetScale, _targetDuration)
                        .OnComplete(() => { _owner.transform.localScale = Vector3.one; })
                        .SetEase(Ease.Linear);
    }
}
