using Lean.Pool;
using UnityEngine;

public class ClickLaserSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private ClickLaser _laserPrefab;
    [SerializeField] [Range(0f, 1f)] private float _spawnChance = 0.1f;
    [SerializeField] private double _baseDamage = 10;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    private LeanGameObjectPool _pool;

    private void Awake()
    {
        _pool = GetComponent<LeanGameObjectPool>();
    }

    private void OnEnable()
    {
        Clicker.OnClicked += OnPlayerClicked;
    }

    private void OnDisable()
    {
        Clicker.OnClicked -= OnPlayerClicked;
    }

    private void OnPlayerClicked(ClickInfo clickInfo)
    {
        if (Random.value > _spawnChance) return;

        SpawnLaser();
    }

    private void SpawnLaser()
    {
        if (_laserPrefab == null) return;

        (Vector2 startPos, Vector2 endPos) = ScreenEdgeUtility.GetRandomEdgePositions();

        GameObject laserObj = _pool.Spawn(startPos, Quaternion.identity);
        ClickLaser laser = laserObj.GetComponent<ClickLaser>();
        laser.Initialize(startPos, endPos, _baseDamage, this);
        _audioSource.Play();
    }

    public void DespawnLaser(ClickLaser laser)
    {
        _pool.Despawn(laser.gameObject);
    }
}