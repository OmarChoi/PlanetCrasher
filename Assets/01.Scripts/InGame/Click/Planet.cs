using System;
using UnityEngine;

public class Planet : MonoBehaviour, IClickable
{
    private static readonly int _progress = Shader.PropertyToID("_Progress");
    [SerializeField] private PlanetData _planetData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private double _health = 1;
    private double _maxHealth = 1;
    private const double HealthGrowthRate = 1.15;
    private IFeedback[] _feedbacks;
    private Material _crackMaterialInstance;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>();
        UpdateCrackProgress();
    }

    private double GetMaxHealth(int currentPlanetIndex)
    {
        double baseHealth = _planetData.MaxHealth;
        return baseHealth * Math.Pow(HealthGrowthRate, currentPlanetIndex);
    }
    
    public void Init(PlanetData data, int currentPlanetIndex)
    {
        _planetData = data;

        _maxHealth = GetMaxHealth(currentPlanetIndex);
        _health = _maxHealth;

        if (_spriteRenderer == null || _planetData.PlanetMaterial == null) return;
        _crackMaterialInstance = Instantiate<Material>(_planetData.PlanetMaterial);
        _spriteRenderer.material = _crackMaterialInstance;
        UpdateCrackProgress();
        
        gameObject.SetActive(true);
    }
    
    public bool OnClick(ClickInfo clickInfo)
    {
        if (_health <= 0f) return false;
        
        _health -= clickInfo.Damage;
        GameManager.Instance.AddGold(clickInfo.Damage);
        
        if (_health <= 0f)
        {
            _health = 0f;
            Explode();
            return true;
        }
        
        foreach (IFeedback feedback in _feedbacks)
        {
            feedback.Play(clickInfo);
        }

        UpdateCrackProgress();

        return true;
    }

    private void Explode()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ChangePlanet();
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
        transform.Rotate(0, 0, _planetData.RotationSpeed * hpPercent * Time.deltaTime);
    }
}
