using System.Collections.Generic;
using UnityEngine;

public class ClickLaser : MonoBehaviour
{
    private enum LaserState
    {
        Extending,
        Retracting
    }

    [Header("Movement")]
    [SerializeField] private float _speed = 15f;

    [Header("Laser Visual")]
    [SerializeField] private LineRenderer _lineRenderer;

    [Header("Damage")]
    [SerializeField] private LayerMask _targetLayer;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private double _damage;
    private ClickLaserSpawner _spawner;

    private bool _isInitialized;
    private LaserState _state;
    private float _headProgress;
    private float _tailProgress;
    private float _totalDistance;

    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[10];
    private readonly HashSet<Collider2D> _hitTargets = new HashSet<Collider2D>();
    private ContactFilter2D _contactFilter;

    private void Awake()
    {
        _contactFilter = new ContactFilter2D();
        _contactFilter.SetLayerMask(_targetLayer);
        _contactFilter.useLayerMask = true;
        _contactFilter.useTriggers = true;
    }

    public void Initialize(Vector2 startPos, Vector2 endPos, double damage, ClickLaserSpawner spawner)
    {
        _startPosition = startPos;
        _endPosition = endPos;
        _damage = damage;
        _spawner = spawner;

        _totalDistance = Vector2.Distance(startPos, endPos);
        _headProgress = 0f;
        _tailProgress = 0f;
        _state = LaserState.Extending;

        _hitTargets.Clear();
        _isInitialized = true;

        UpdateLineRenderer();
    }

    private void Update()
    {
        if (!_isInitialized) return;

        float delta = _speed * Time.deltaTime;

        switch (_state)
        {
            case LaserState.Extending:
                _headProgress += delta;
                if (_headProgress >= _totalDistance)
                {
                    _headProgress = _totalDistance;
                    _state = LaserState.Retracting;
                }
                break;

            case LaserState.Retracting:
                _tailProgress += delta;
                if (_tailProgress >= _totalDistance)
                {
                    Despawn();
                    return;
                }
                break;
        }

        UpdateLineRenderer();
        CheckDamage();
    }

    private void UpdateLineRenderer()
    {
        if (_lineRenderer == null) return;

        Vector2 headPos = Vector2.Lerp(_startPosition, _endPosition, _headProgress / _totalDistance);
        Vector2 tailPos = Vector2.Lerp(_startPosition, _endPosition, _tailProgress / _totalDistance);

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, tailPos);
        _lineRenderer.SetPosition(1, headPos);
    }

    private void CheckDamage()
    {
        Vector2 headPos = Vector2.Lerp(_startPosition, _endPosition, _headProgress / _totalDistance);
        Vector2 tailPos = Vector2.Lerp(_startPosition, _endPosition, _tailProgress / _totalDistance);

        int hitCount = Physics2D.Linecast(tailPos, headPos, _contactFilter, _hitBuffer);
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = _hitBuffer[i];
            if (hit.collider == null) continue;
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

    private void Despawn()
    {
        _isInitialized = false;
        _spawner.DespawnLaser(this);
    }
}