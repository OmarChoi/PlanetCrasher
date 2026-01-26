using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFloaterFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] Canvas _canvas;
    [SerializeField] TextMeshProUGUI _damageTextPrefab;
    [SerializeField] int _poolSize = 20;

    private ObjectPool<TextMeshProUGUI> _damagePool;

    private void Awake()
    {
        _damagePool = new ObjectPool<TextMeshProUGUI>(_damageTextPrefab, _canvas.transform, _poolSize);
    }

    public void Play(ClickInfo clickInfo)
    {
        TextMeshProUGUI textUI = _damagePool.Get();
        RectTransform objectTransform = textUI.rectTransform;
        objectTransform.position = clickInfo.Position;
        textUI.SetText(clickInfo.Damage.ToString());

        objectTransform.DOLocalMoveY(objectTransform.localPosition.y + 100f, 1.0f)
                       .OnComplete(() => { _damagePool.Return(textUI); })
                       .SetEase(Ease.Linear);
    }
}
