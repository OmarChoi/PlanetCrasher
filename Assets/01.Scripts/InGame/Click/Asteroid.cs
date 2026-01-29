using UnityEngine;

public struct AsteroidInfo
{
    public Vector2 Position;
    public Vector2 EndPosition;
    public float Speed;

    public AsteroidInfo(Vector2 position, Vector2 endPosition, float speed)
    {
        Position = position;
        EndPosition = endPosition;
        Speed = speed;
    }
}

public class Asteroid : MonoBehaviour, IClickable
{
    private AsteroidInfo _asteroidInfo;
    private IFeedback[] _feedbacks;
    private Vector2 _direction;
    private AsteroidSpawner _spawner;
    private bool _isActive;

    
    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>();
    }

    public void Init(AsteroidInfo info, AsteroidSpawner spawner)
    {
        _isActive = true;
        _asteroidInfo = info;
        _spawner = spawner;
        transform.position = _asteroidInfo.Position;
        _direction = (_asteroidInfo.EndPosition - _asteroidInfo.Position).normalized;
    }

    private void Update()
    {
        if (!_isActive) return;

        transform.position += (Vector3)_direction * (_asteroidInfo.Speed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.z = -1f;
        transform.position = pos;

        if (Vector2.Distance(transform.position, _asteroidInfo.EndPosition) < 0.1f)
        {
            _spawner.ReleaseAsteroid(this);
        }
    }
    
    public bool OnClick(ClickInfo clickInfo)
    {
        _isActive = false;
        foreach (IFeedback feedback in _feedbacks)
        {
            feedback.Play(clickInfo);
        }
        _spawner.PlaySFX(clickInfo);
        _spawner.ReleaseAsteroid(this);
        return true;
    }
}
