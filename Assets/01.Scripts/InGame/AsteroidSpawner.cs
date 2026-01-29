using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private List<Asteroid> _asteroidPrefabs;

    private void Awake()
    {
        StartCoroutine(Spawn_Coroutine());
    }

    private IEnumerator Spawn_Coroutine()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(2f);
        }
    }
    
    private void SpawnAsteroid()
    {
        int randomIndex = Random.Range(0, _asteroidPrefabs.Count);
        Asteroid prefab = _asteroidPrefabs[randomIndex];

        Asteroid asteroid = LeanPool.Spawn(prefab, transform);

        (Vector2 position, Vector2 endPosition) = ScreenEdgeUtility.GetRandomEdgePositions();
        float speed = Random.Range(1f, 3f);

        var info = new AsteroidInfo(position, endPosition, speed);
        asteroid.Init(info, this);
    }

    public void ReleaseAsteroid(Asteroid asteroid, float duration = 0f)
    {
        LeanPool.Despawn(asteroid, duration);
    }
}
