using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        CurrencyManager.OnDataChanged -= OnGoldChanged;
        CurrencyManager.OnDataChanged += OnGoldChanged;
    }

    private void OnDisable()
    {
        CurrencyManager.OnDataChanged -= OnGoldChanged;
    }

    private void OnGoldChanged()
    {
        _text.text = CurrencyManager.Instance.Gold.ToString();
    }
}
