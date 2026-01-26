using UnityEngine;

public class Satellite : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private float _orbitSpeed = 50f;
    [SerializeField] private float _orbitDistance = 1.65f;
    private float _angle;
    
    private void Start()
    {
        UpdatePosition();
    }

    private void Update()
    {
        _angle += _orbitSpeed * Time.deltaTime;
        
        UpdatePosition();
        UpdateRotation();
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
}