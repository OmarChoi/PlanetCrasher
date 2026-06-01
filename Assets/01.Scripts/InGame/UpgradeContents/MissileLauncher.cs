using Lean.Pool;
using UnityEngine;

public class MissileLauncher : UpgradeContent
{
    protected override EUpgradeType UpgradeType => EUpgradeType.Missile;
    protected override bool DeactivateWhenUnowned => true;

    [Header("Target")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _targetRadius = 1f;

    [Header("Base Settings")]
    [SerializeField] private float _baseShootInterval = 3f;
    [SerializeField] private AudioClip _shootSfx;

    [Header("Spawn Position")]
    [SerializeField] private float _viewportHeight = 0.3f;
    [SerializeField] private float _viewportMargin = 0.1f;

    [SerializeField] private ParticleSystem _particleSystem;

    // 런타임 계산 값
    private double _damage;
    private double _shootInterval;

    private float _shootTimer;
    private bool _spawnFromLeft = true;
    private LeanGameObjectPool _pool;
    private Camera _mainCamera;

    protected override void RefreshStats()
    {
        _damage = GetEffectValue(EUpgradeEffectType.Damage);

        // CooldownReduction은 0~1 분수. _baseShootInterval에 곱해 발사 간격을 단축한다.
        double cooldownReduction = GetEffectValue(EUpgradeEffectType.CooldownReduction);
        _shootInterval = _baseShootInterval * (1f - Mathf.Clamp01((float)cooldownReduction));
    }

    protected override void Init()
    {
        _pool = GetComponent<LeanGameObjectPool>();
        _mainCamera = Camera.main;
        _shootInterval = _baseShootInterval;
    }

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        if (_shootTimer >= _shootInterval)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_target == null) return;

        Vector3 spawnPosition = GetSpawnPosition();
        SpawnMissile(spawnPosition);

        if (_shootSfx != null)
        {
            SoundManager.Instance.PlaySfx(_shootSfx);
        }

        _spawnFromLeft = !_spawnFromLeft;
    }

    private Vector3 GetSpawnPosition()
    {
        float viewportX = _spawnFromLeft ? _viewportMargin : 1f - _viewportMargin;
        float cameraDistance = Mathf.Abs(_mainCamera.transform.position.z - _target.position.z);

        Vector3 viewportPoint = new Vector3(viewportX, _viewportHeight, cameraDistance);
        return _mainCamera.ViewportToWorldPoint(viewportPoint);
    }

    private void SpawnMissile(Vector3 spawnPosition)
    {
        Vector3 direction = (_target.position - spawnPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject missileObj = _pool.Spawn(spawnPosition, rotation);
        if (missileObj.TryGetComponent(out Missile missile))
        {
            Vector2 randomOffset = Random.insideUnitCircle * _targetRadius;
            Vector3 destination = _target.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            missile.Initialize(_target, _damage, destination, _particleSystem, this);
        }
    }

    public void DespawnMissile(Missile missile)
    {
        _pool.Despawn(missile.gameObject);
    }
}
