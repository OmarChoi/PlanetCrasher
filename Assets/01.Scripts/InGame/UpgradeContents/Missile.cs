using DG.Tweening;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _explosionSfx;

    [Header("Scale Effect")]
    [SerializeField] private float _endScaleMultiplier = 0.3f;
    [SerializeField] private Ease _scaleEase = Ease.InQuad;

    [Header("Tilt Effect")]
    [SerializeField] private float _tiltAngle = 45f;

    private Transform _target;
    private IClickable _targetClickable;
    private Vector3 _destination;
    private double _damage;
    private MissileLauncher _launcher;
    private bool _isDespawned;

    private Vector3 _startScale;
    private float _initialDistance;
    private float _yTilt;

    public void Initialize(Transform target, IClickable targetClickable, double damage, float targetRadius, MissileLauncher launcher)
    {
        _target = target;
        _targetClickable = targetClickable;
        _damage = damage;
        _launcher = launcher;
        _isDespawned = false;

        Vector2 randomOffset = Random.insideUnitCircle * targetRadius;
        _destination = target.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        _startScale = transform.localScale;
        _initialDistance = Vector3.Distance(transform.position, _destination);

        // 왼쪽에서 오면 +, 오른쪽에서 오면 -
        _yTilt = transform.position.x < _destination.x ? _tiltAngle : -_tiltAngle;
    }

    private void Update()
    {
        if (_isDespawned) return;

        if (_target == null)
        {
            Despawn();
            return;
        }

        Vector3 direction = (_destination - transform.position).normalized;
        transform.position += direction * (_speed * Time.deltaTime);

        float angleToTarget = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, _yTilt, angleToTarget);

        UpdateScale();

        float currentDistance = Vector3.Distance(transform.position, _destination);
        if (currentDistance <= _speed * Time.deltaTime)
        {
            OnHit();
        }
    }

    private void UpdateScale()
    {
        float currentDistance = Vector3.Distance(transform.position, _destination);
        float progress = 1f - (currentDistance / _initialDistance);
        float scaleMultiplier = DOVirtual.EasedValue(1f, _endScaleMultiplier, progress, _scaleEase);
        transform.localScale = _startScale * scaleMultiplier;
    }

    private void OnHit()
    {
        if (_targetClickable != null)
        {
            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.AutoClick,
                Damage = _damage,
                Position = _destination,
                EffectParticle = _hitEffect
            };
            _targetClickable.OnClick(clickInfo);
        }

        if (_explosionSfx != null)
        {
            SoundManager.Instance.PlaySfx(_explosionSfx);
        }

        Despawn();
    }

    private void Despawn()
    {
        if (_isDespawned) return;
        _isDespawned = true;
        _launcher.DespawnMissile(this);
    }
}
