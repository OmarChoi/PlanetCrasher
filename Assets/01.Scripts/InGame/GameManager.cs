using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private AudioSource _audioSource;
    
    [SerializeField] private double _manualDamage = 1;
    [SerializeField] private double _autoDamage = 1;
    [SerializeField] private ParticleSystem _explodeParticles;
    
    
    [Header("Planets")]
    [SerializeField] private GameObject _planetPrefab;
    [SerializeField] private List<PlanetData> _planetDatas;
    private Planet _currentPlanet;
    private int _currentPlanetIndex = 0;
    
    public double ManualDamage => _manualDamage;
    public double AutoDamage => _autoDamage;
    public static event Action<double> OnAutoDamageChanged;
    public static event Action OnUpgradeCompleted;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        _currentPlanet = Instantiate(_planetPrefab, transform).GetComponent<Planet>();
        _currentPlanet.gameObject.SetActive(false);
        _currentPlanet.Init(_planetDatas[_currentPlanetIndex], _currentPlanetIndex);
        
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        OnAutoDamageChanged?.Invoke(_autoDamage);
        OnUpgradeCompleted?.Invoke();

        _currentPlanet.Init(_planetDatas[_currentPlanetIndex], _currentPlanetIndex);
    }

    public void ChangePlanet()
    {
        _explodeParticles.Play();
        _audioSource.Play();
        
        _currentPlanetIndex++;
        int planetIndex = (_currentPlanetIndex) % _planetDatas.Count;
        _currentPlanet.Init(_planetDatas[planetIndex], _currentPlanetIndex);
    }
}
