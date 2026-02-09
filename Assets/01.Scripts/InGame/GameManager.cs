using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ParticleSystem _explodeParticles;

    [Header("Planets")]
    [SerializeField] private List<PlanetData> _planetDatas;
    [SerializeField] private Planet _currentPlanet;
    private int _currentPlanetIndex = 0;

    protected override void Initialize()
    {
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
