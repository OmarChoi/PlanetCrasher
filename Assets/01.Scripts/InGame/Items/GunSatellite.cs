using Lean.Pool;
using UnityEngine;

public class GunSatellite : MonoBehaviour
{
    [Header("Satellite")]
    [SerializeField] private Transform _parent;
    [SerializeField] private float _orbitSpeed;
    [SerializeField] private float _orbitDistance;

    [Header("Shooting")]
    [SerializeField] private float _shootInterval = 1f;
    [SerializeField] private double _damage = 10;
    [SerializeField] private Transform[] _firePoints;

    private float _angle = 180.0f;
    private float _shootTimer;
    private LeanGameObjectPool _pool;

    private void Awake()
    {
        _pool = GetComponent<LeanGameObjectPool>();
    }

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
        Vector3 direction = _parent.position - transform.position;                                 
        float angleToParent = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;               
        transform.rotation = Quaternion.Euler(0, 0, angleToParent);
    }

    private void UpdatePosition()
    {
        float radian = _angle * Mathf.Deg2Rad;

        float x = _parent.position.x + Mathf.Cos(radian) * _orbitDistance;
        float y = _parent.position.y + Mathf.Sin(radian) * _orbitDistance;

        transform.position = new Vector3(x, y, _parent.position.z);
    }

    private void Shoot()
    {
        if (_parent == null) return;

        if (_firePoints == null || _firePoints.Length == 0)
        {
            SpawnBullet(transform.position);
        }
        else
        {
            foreach (Transform firePoint in _firePoints)
            {
                if (firePoint != null)
                {
                    SpawnBullet(firePoint.position);
                }
            }
        }
    }

    private void SpawnBullet(Vector3 spawnPosition)
    {
        GameObject bulletObj = _pool.Spawn(spawnPosition, transform.rotation);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(_parent, _damage, this);
        }
    }

    public void DespawnBullet(Bullet bullet)
    {
        _pool.Despawn(bullet.gameObject);
    }
}