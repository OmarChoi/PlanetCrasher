using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        Refresh();
    }
    
    private void OnEnable()
    {
        CurrencyManager.OnDataChanged -= Refresh;
        CurrencyManager.OnDataChanged += Refresh;
    }

    private void OnDisable()
    {
        CurrencyManager.OnDataChanged -= Refresh;
    }

    private void Refresh()
    {
        _text.text = CurrencyManager.Instance.Gold.ToString();
    }
}
