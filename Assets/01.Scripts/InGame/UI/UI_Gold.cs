using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        GameManager.OnGoldChanged -= OnGoldChanged;
        GameManager.OnGoldChanged += OnGoldChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGoldChanged -= OnGoldChanged;
    }

    private void OnGoldChanged(double gold)
    {
        _text.text = gold.ToFormattedString();
    }
}
