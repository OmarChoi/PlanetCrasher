using System;
using UnityEngine;

public class Planet : MonoBehaviour, IClickable
{
    private static readonly int _progress = Shader.PropertyToID("_Progress");
    [SerializeField] private PlanetData _planetData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _explodeSfx;

    private double _health = 1;
    private double _maxHealth = 1;
    private IFeedback[] _feedbacks;
    private Material _crackMaterialInstance;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>();
        UpdateCrackProgress();
    }

    private double GetMaxHealth(int currentPlanetIndex, double healthGrowthRate)
    {
        double baseHealth = _planetData.MaxHealth;
        return baseHealth * Math.Pow(healthGrowthRate, currentPlanetIndex);
    }

    public void Init(PlanetData data, int currentPlanetIndex, double healthGrowthRate)
    {
        _planetData = data;

        _maxHealth = GetMaxHealth(currentPlanetIndex, healthGrowthRate);
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
        CurrencyManager.Instance.Add(ECurrencyType.Gold, clickInfo.Damage);
        
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
        SoundManager.Instance.PlaySfx(_explodeSfx);
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
