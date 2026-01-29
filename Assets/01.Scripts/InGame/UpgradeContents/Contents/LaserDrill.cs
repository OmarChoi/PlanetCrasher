using UnityEngine;

public enum ESatelliteType
{
    Beam,
    Projectile
}

public class LaserDrill : MonoBehaviour
{
    [Header("Satellite")]
    [SerializeField] private Transform _parent;
    [SerializeField] private float _orbitSpeed = 50f;
    [SerializeField] private float _orbitDistance = 1.65f;
    
    [Space(10)]
    [Header("Laser")]
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private float _beamDistance = 1f;
    [SerializeField] private int _beamSegments = 20;
    [SerializeField] private float _beamSpace = 0.25f;
    [SerializeField] private float _waveAmplitude = 0.1f;
    [SerializeField] private float _waveFrequency = 5f;
    [SerializeField] private float _waveSpeed = 5f;
    
    [Space(10)]
    [Header("Damage")]
    [SerializeField] private float _damage = 20f;
    
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
        UpdateBeam();
    }
    
    private void UpdateBeam()
    {
        if (_beam == null) return;

        _beam.positionCount = _beamSegments;

        Vector3 startPos = new Vector3(_beamSpace, 0, 0);
        Vector3 endPos = new Vector3(_beamDistance, 0, 0);

        for (int i = 0; i < _beamSegments; i++)
        {
            float t = i / (float)(_beamSegments - 1);

            Vector3 basePos = Vector3.Lerp(startPos, endPos, t);
            float wave = Mathf.Sin(t * _waveFrequency * Mathf.PI + Time.time * _waveSpeed) * _waveAmplitude;

            basePos.y += wave;

            _beam.SetPosition(i, basePos);
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
}