using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private ParticleSystem _hitEffect;

    private Transform _target;
    private double _damage;
    private GunSatellite _gunSatellite;
    private bool _isDespawned;

    public void Initialize(Transform target, double damage, GunSatellite gunSatellite)
    {
        _target = target;
        _damage = damage;
        _gunSatellite = gunSatellite;
        _isDespawned = false;
    }

    private void Update()
    {
        if (_isDespawned) return;

        if (_target == null)
        {
            Despawn();
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * (_speed * Time.deltaTime);

        float angleToTarget = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleToTarget);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Clickable"))
        {
            OnHit(collision.transform);
        }
    }

    private void OnHit(Transform hitTarget)
    {
        IClickable clickable = hitTarget.GetComponent<IClickable>();
        if (clickable != null)
        {
            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.AutoClick,
                Damage = _damage,
                Position = transform.position,
                EffectParticle = _hitEffect
            };
            clickable.OnClick(clickInfo);
        }

        Despawn();
    }

    private void Despawn()
    {
        if (_isDespawned) return;
        _isDespawned = true;
        _gunSatellite.DespawnBullet(this);
    }
}
