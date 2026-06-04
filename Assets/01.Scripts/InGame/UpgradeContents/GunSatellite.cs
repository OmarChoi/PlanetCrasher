using Lean.Pool;
using UnityEngine;

public class GunSatellite : UpgradeContent
{
    protected override EUpgradeType UpgradeType => EUpgradeType.Satellite;

    [Header("Satellite")]
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private float _orbitDistance;

    [Header("Base Settings")]
    [SerializeField] private float _baseShootInterval = 1f;
    [SerializeField] private float _minShootInterval = 0.1f;
    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private AudioClip _shootSfx;

    private Transform _target;

    // 런타임 계산 값
    private double _damage;
    private float _shootInterval;

    private float _angle = 180.0f;
    private float _shootTimer;
    private LeanGameObjectPool _pool;

    public override void Bind(Planet planet) => _target = planet.transform;

    protected override void RefreshStats()
    {
        _damage = GetEffectValue(EUpgradeEffectType.Damage);

        // interval이 0/음수로 떨어져 매 프레임 발사로 회귀하지 않도록 하한선을 적용한다.
        double cooldownReduction = GetEffectValue(EUpgradeEffectType.CooldownReduction);
        float reducedInterval = _baseShootInterval * (1f - Mathf.Clamp01((float)cooldownReduction));
        _shootInterval = Mathf.Max(reducedInterval, _minShootInterval);
    }

    protected override void Init()
    {
        _pool = GetComponent<LeanGameObjectPool>();
        _shootInterval = _baseShootInterval;
    }

    // _target은 스포너가 Bind로 주입하므로 첫 위치 계산은 Awake 체인이 아닌 Start에서 수행한다.
    private void Start()
    {
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
