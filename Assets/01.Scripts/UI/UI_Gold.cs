using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private bool _isDirty;

    private void OnEnable()
    {
        CurrencyManager.OnDataChanged -= MarkDirty;
        CurrencyManager.OnDataChanged += MarkDirty;
        _isDirty = true;
    }

    private void OnDisable()
    {
        CurrencyManager.OnDataChanged -= MarkDirty;
    }

    private void LateUpdate()
    {
        if (!_isDirty) return;
        _isDirty = false;
        _text.SetText(CurrencyManager.Instance.Gold.ToString());
    }

    private void MarkDirty()
    {
        _isDirty = true;
    }
}
