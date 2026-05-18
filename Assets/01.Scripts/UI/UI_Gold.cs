using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private bool _isDirty;
    private double _lastDisplayedValue = double.NaN;

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

        Currency gold = CurrencyManager.Instance.Gold;
        if (gold.Value == _lastDisplayedValue) return;

        _lastDisplayedValue = gold.Value;
        _text.SetText(gold.ToString());
    }

    private void MarkDirty()
    {
        _isDirty = true;
    }
}
