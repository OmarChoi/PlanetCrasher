using UnityEngine;

public class Planet : MonoBehaviour, IClickable
{
    private static readonly int _progress = Shader.PropertyToID("_Progress");
    [SerializeField] private string _planetName;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private PlanetData _planetData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private double _health;
    private double _maxHealth;
    private IFeedback[] _feedbacks;
    private Material _crackMaterialInstance;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>();

        if (_planetData != null)
        {
            _maxHealth = _planetData.maxHealth;
            _health = _maxHealth;

            if (_spriteRenderer != null && _planetData.crackMaterial != null)
            {
                _crackMaterialInstance = Instantiate<Material>(_planetData.crackMaterial);
                _spriteRenderer.material = _crackMaterialInstance;
            }

            if (_spriteRenderer != null && _planetData.planetImage != null)
            {
                _spriteRenderer.sprite = _planetData.planetImage;
            }
        }

        UpdateCrackProgress();
    }
    
    public bool OnClick(ClickInfo clickInfo)
    {
        foreach (IFeedback feedback in _feedbacks)
        {
            feedback.Play(clickInfo);
        }

        _health -= clickInfo.Damage;
        if (_health < 0f) _health = 0f;

        UpdateCrackProgress();

        GameManager.Instance.AddGold(clickInfo.Damage);
        return true;
    }

    private void UpdateCrackProgress()
    {
        if (_crackMaterialInstance != null && _maxHealth > 0)
        {
            double progress = 1f - (_health / _maxHealth);
            _crackMaterialInstance.SetFloat(_progress, (float)progress);
        }
    }

    private void Update()
    {
        float hpPercent = (float)(_health / _maxHealth);
        transform.Rotate(0, 0, _rotationSpeed * hpPercent * Time.deltaTime);
    }
}
