using System;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public static event Action<ClickInfo> OnClicked;

    private const int MaxHits = 10;

    private Camera _mainCamera;
    [SerializeField] private LayerMask _clickableLayer;
    [SerializeField] private AudioClip _clickSfx;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[MaxHits];

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
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

        ClickInfo clickInfo = new ClickInfo
        {
            Type = EClickType.PerClick,
            Target = clickable,
            Position = mousePos,
        };
        
        OnClicked?.Invoke(clickInfo);
        SoundManager.Instance.PlaySfx(_clickSfx);
    }
}
