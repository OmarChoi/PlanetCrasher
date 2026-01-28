using UnityEngine;

public class Clicker : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameObject _clickParticlePrefab; 
    private ParticleSystem _clickParticle;
    private AudioSource _clickAudio;

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
    
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit != true) return;
        if (hit.collider.TryGetComponent(out IClickable clickable))
        {
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
        }
    }
}
