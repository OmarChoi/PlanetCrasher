using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private ParticleSystem _hitEffect;

    private Transform _target;
    private double _damage;
    private const float HitDistance = 0.1f;

    public void Initialize(Transform target, double damage)
    {
        _target = target;
        _damage = damage;
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.position += direction * _speed * Time.deltaTime;

        float angleToTarget = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleToTarget);

        if (Vector3.Distance(transform.position, _target.position) < HitDistance)
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        IClickable clickable = _target.GetComponent<IClickable>();
        if (clickable != null)
        {
            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.AutoClick,
                Damage = _damage,
                Position = transform.position,
                EffectParticle = null
            };
            clickable.OnClick(clickInfo);
        }

        if (_hitEffect != null)
        {
            Instantiate(_hitEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
