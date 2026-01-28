using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private List<Asteroid> _asteroidPrefabs;
    
    // Audio
    private AudioSource _audioSource;
    private AudioClip _audioClip;
    
    // SpawnPos
    private Camera _mainCamera;
    private Vector2 _screenCenter;
    private Vector2 _topLeft;
    private Vector2 _topRight;
    private Vector2 _bottomLeft;
    private Vector2 _bottomRight;
    private float _screenWidth;
    private float _screenHeight;

    private void Awake()
    {
        _mainCamera = Camera.main;
        float nearClipPlane = _mainCamera.nearClipPlane;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        _topLeft = _mainCamera.ScreenToWorldPoint(new Vector3(0f, screenHeight, nearClipPlane));
        _topRight = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, nearClipPlane));
        _bottomLeft = _mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, nearClipPlane));
        _bottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, 0f, nearClipPlane));
        _screenCenter = _mainCamera.ScreenToWorldPoint(new Vector3(screenWidth / 2f, screenHeight / 2f, nearClipPlane));
        _screenWidth = _topRight.x - _topLeft.x;
        _screenHeight = _topLeft.y - _bottomLeft.y;

        _audioSource = GetComponent<AudioSource>();
        _audioClip = _audioSource.clip;
        
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

        (Vector2 position, Vector2 endPosition) = GetRandomEdgePositions();
        float speed = Random.Range(1f, 3f);

        var info = new AsteroidInfo(position, endPosition, speed);
        asteroid.Init(info, this);
    }

    private (Vector2 startPos, Vector2 endPos) GetRandomEdgePositions()
    {
        int edge = Random.Range(0, 4);
        float offset = 0.1f;

        return edge switch
        {
            0 => (new Vector2(Random.Range(_topLeft.x, _topRight.x), _topLeft.y + _screenHeight * offset),
                  new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y - _screenHeight * offset)),
            1 => (new Vector2(Random.Range(_bottomLeft.x, _bottomRight.x), _bottomLeft.y - _screenHeight * offset),
                  new Vector2(Random.Range(_topLeft.x, _topRight.x), _topLeft.y + _screenHeight * offset)),
            2 => (new Vector2(_topLeft.x - _screenWidth * offset, Random.Range(_bottomLeft.y, _topLeft.y)),
                  new Vector2(_topRight.x + _screenWidth * offset, Random.Range(_bottomRight.y, _topRight.y))),
            3 => (new Vector2(_topRight.x + _screenWidth * offset, Random.Range(_bottomRight.y, _topRight.y)),
                  new Vector2(_topLeft.x - _screenWidth * offset, Random.Range(_bottomLeft.y, _topLeft.y))),
            _ => (Vector2.zero, Vector2.zero)
        };
    }

    public void ReleaseAsteroid(Asteroid asteroid, float duration = 0f)
    {
        LeanPool.Despawn(asteroid, duration);
    }
    public void PlaySFX(ClickInfo clickInfo)
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
