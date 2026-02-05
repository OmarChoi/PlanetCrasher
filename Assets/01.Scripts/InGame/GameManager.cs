using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    [SerializeField] private ParticleSystem _explodeParticles;
    
    [Header("Planets")]
    [SerializeField] private List<PlanetData> _planetDatas;
    [SerializeField] private Planet _currentPlanet;
    private int _currentPlanetIndex = 0;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        _currentPlanet.Init(_planetDatas[_currentPlanetIndex], _currentPlanetIndex);
    }

    public void ChangePlanet()
    {
        _explodeParticles.Play();

        _currentPlanetIndex++;
        int planetIndex = (_currentPlanetIndex) % _planetDatas.Count;
        _currentPlanet.Init(_planetDatas[planetIndex], _currentPlanetIndex);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
