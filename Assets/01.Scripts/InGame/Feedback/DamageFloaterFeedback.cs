using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFloaterFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _damageTextPrefab;
    [SerializeField] private int _poolSize = 20;

    private ObjectPool<TextMeshProUGUI> _damagePool;

    private void Awake()
    {
        _damagePool = new ObjectPool<TextMeshProUGUI>(_damageTextPrefab, _canvas.transform, _poolSize);
    }

    public void Play(ClickInfo clickInfo)
    {
        DamageFloaterSpawner.Instance.ShowDamage(clickInfo);
        // TextMeshProUGUI textUI = _damagePool.Get();
        // RectTransform objectTransform = textUI.rectTransform;
        // objectTransform.position = clickInfo.Position;
        // textUI.SetText(clickInfo.Damage.ToString());
// 
        // Sequence seq = DOTween.Sequence();
        // objectTransform.DOLocalMoveY(objectTransform.localPosition.y + 100f, 1.0f)
        //                .OnComplete(() => { _damagePool.Return(textUI); })
        //                .SetEase(Ease.Linear);
    }
}
