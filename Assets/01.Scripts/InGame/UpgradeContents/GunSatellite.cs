using Lean.Pool;
using UnityEngine;

public class GunSatellite : UpgradeContent
{
    protected override EUpgradeType UpgradeType => EUpgradeType.Satellite;
    protected override bool DeactivateWhenUnowned => true;

    [Header("Satellite")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private float _orbitDistance;

    [Header("Base Settings")]
    [SerializeField] private float _baseShootInterval = 1f;
    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private AudioClip _shootSfx;

    // 런타임 계산 값
    private double _damage;
    private float _shootInterval;

    private float _angle = 180.0f;
    private float _shootTimer;
    private LeanGameObjectPool _pool;

    protected override void RefreshStats()
    {
        _damage = GetEffectValue(EUpgradeEffectType.Damage);

        double cooldownReduction = GetEffectValue(EUpgradeEffectType.CooldownReduction);
        _shootInterval = _baseShootInterval * (1f - (float)cooldownReduction);
    }

    protected override void Init()
    {
        _pool = GetComponent<LeanGameObjectPool>();
    }

    protected override void InitializeUpgradeData()
    {
        base.InitializeUpgradeData();
        UpdatePosition();
    }

    private void Update()
    {
        _angle += _orbitSpeed * Time.deltaTime;

        UpdatePosition();
        UpdateRotation();

        _shootTimer += Time.deltaTime;
        if (_shootTimer >= _shootInterval)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    private void UpdateRotation()
    {
        Vector3 direction = _target.position - transform.position;
        float angleToParent = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleToParent);
    }

    private void UpdatePosition()
    {
        float radian = _angle * Mathf.Deg2Rad;

        float x = _target.position.x + Mathf.Cos(radian) * _orbitDistance;
        float y = _target.position.y + Mathf.Sin(radian) * _orbitDistance;

        transform.position = new Vector3(x, y, _target.position.z);
    }

    private void Shoot()
    {
        if (_target == null) return;
        foreach (Transform firePoint in _firePoints)
        {
            if (firePoint != null)
            {
                SpawnBullet(firePoint.position);
            }
        }
        SoundManager.Instance.PlaySfx(_shootSfx);
    }

    private void SpawnBullet(Vector3 spawnPosition)
    {
        GameObject bulletObj = _pool.Spawn(spawnPosition, transform.rotation);
        if (bulletObj.TryGetComponent(out Bullet bullet))
        {
            bullet.Initialize(_target, _damage, this);
        }
    }

    public void DespawnBullet(Bullet bullet)
    {
        _pool.Despawn(bullet.gameObject);
    }
}
