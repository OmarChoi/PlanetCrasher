using UnityEngine;

public class Clicker : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
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
    
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit != true) return;
        if (hit.collider.TryGetComponent(out IClickable clickable))
        {
            clickable.OnClick();
        }
    }
}
