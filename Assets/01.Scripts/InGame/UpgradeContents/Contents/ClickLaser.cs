using System.Collections.Generic;
using UnityEngine;

public class ClickLaser : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 15f;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _laserLength = 2f;

    [Header("Damage")]
    [SerializeField] private LayerMask _targetLayer;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;
    private double _damage;
    private ClickLaserSpawner _spawner;

    private bool _isInitialized;

    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[10];
    private readonly HashSet<Collider2D> _hitTargets = new HashSet<Collider2D>();
    private ContactFilter2D _contactFilter;

    private void Awake()
    {
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(_targetLayer);
        _contactFilter.useLayerMask = true;
    }

    public void Initialize(Vector2 startPos, Vector2 endPos, double damage, ClickLaserSpawner spawner)
    {
        _startPosition = startPos;
        _endPosition = endPos;
        _damage = damage;
        _spawner = spawner;

        transform.position = startPos;
        _direction = (endPos - startPos).normalized;

        // Rotate to face direction
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        _hitTargets.Clear();
        _isInitialized = true;

        UpdateLineRenderer();
    }

    private void Update()
    {
        if (!_isInitialized) return;

        // Move laser
        MoveLaser();
        
        // Check for damage every frame
        CheckDamage();

        // Check if reached end
        if (HasReachedEnd())
        {
            Despawn();
        }
    }
    
    private void MoveLaser()
    {
        // _lineRenderer.SetPosition(0, Vector3.zero);
        // _lineRenderer.SetPosition(1, new Vector3(_laserLength, 0, 0));
    }

    private void UpdateLineRenderer()
    {
        if (_lineRenderer == null) return;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, new Vector3(_laserLength, 0, 0));
    }

    private void CheckDamage()
    {
        Vector2 currentPos = transform.position;
        Vector2 laserEnd = currentPos + _direction * _laserLength;

        int hitCount = Physics2D.Linecast(currentPos, laserEnd, _contactFilter, _hitBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = _hitBuffer[i];
            if (hit.collider == null) continue;

            // Skip already damaged targets
            if (_hitTargets.Contains(hit.collider)) continue;

            if (hit.collider.TryGetComponent(out IClickable clickable))
            {
                _hitTargets.Add(hit.collider);

                var clickInfo = new ClickInfo
                {
                    Type = EClickType.AutoClick,
                    Damage = _damage,
                    Position = hit.point,
                    EffectParticle = null
                };
                clickable.OnClick(clickInfo);
            }
        }
    }

    private bool HasReachedEnd()
    {
        Vector2 currentPos = transform.position;
        Vector2 toEnd = _endPosition - _startPosition;
        Vector2 toCurrent = currentPos - _startPosition;

        return Vector2.Dot(toCurrent, toEnd) >= toEnd.sqrMagnitude;
    }

    private void Despawn()
    {
        _isInitialized = false;
        _spawner.DespawnLaser(this);
    }
}
