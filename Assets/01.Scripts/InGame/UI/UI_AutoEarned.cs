using TMPro;
using UnityEngine;

public class UI_AutoEarned : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameManager.OnAutoDamageChanged -= UpdateUI;
        GameManager.OnAutoDamageChanged += UpdateUI;
    }

    private void OnDisable()
    {
        GameManager.OnAutoDamageChanged -= UpdateUI;
    }

    private void UpdateUI(double value)
    {
        _text.text = $"{value.ToFormattedString()} /s";
    }
}
