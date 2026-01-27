using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= OnGoldChanged;
            GameManager.Instance.OnGoldChanged += OnGoldChanged;
        }
    }
    
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= OnGoldChanged;
            GameManager.Instance.OnGoldChanged += OnGoldChanged;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGoldChanged -= OnGoldChanged;
        }
    }

    private void OnGoldChanged(double gold)
    {
        _text.SetText(gold.ToString("N0"));
    }
}
