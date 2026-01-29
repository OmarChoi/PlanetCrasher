using System;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public static event Action<ClickInfo> OnClicked;

    private const int MaxHits = 10;

    private Camera _mainCamera;
    [SerializeField] private GameObject _clickParticlePrefab;
    [SerializeField] private LayerMask _clickableLayer;
    private ParticleSystem _clickParticle;
    private AudioSource _clickAudio;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[MaxHits];

    private void Awake()
    {
        _mainCamera = Camera.main;
        GameObject particle = Instantiate(_clickParticlePrefab, this.transform);
        _clickParticle = particle.GetComponent<ParticleSystem>();
        _clickAudio = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryClick(Input.mousePosition);
        }
    }

    private void TryClick(Vector2 mousePosition)
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(mousePosition);

        int hitCount = Physics2D.RaycastNonAlloc(mousePos, Vector2.zero, _hitBuffer, Mathf.Infinity, _clickableLayer);
        if (hitCount == 0) return;

        // IClickable이 있는 오브젝트 중 Z좌표가 가장 작은(카메라에 가장 가까운) 것 선택
        IClickable clickable = null;
        float closestZ = float.MaxValue;

        for (var i = 0; i < hitCount; i++)
        {
            if (!_hitBuffer[i].collider.TryGetComponent(out IClickable candidate)) continue;

            float z = _hitBuffer[i].transform.position.z;
            if (z < closestZ)
            {
                closestZ = z;
                clickable = candidate;
            }
        }

        if (clickable == null) return;
        
        double damage = GameManager.Instance.ManualDamage;
        var info = new ClickInfo
        {
            Type = EClickType.PerClick,
            Damage = damage,
            Position = mousePos,
            EffectParticle = _clickParticle
        };
        clickable.OnClick(info);
        _clickAudio.Play();

        OnClicked?.Invoke(info);
    }
}
