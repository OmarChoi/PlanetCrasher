using Lean.Pool;
using UnityEngine;

public class ClickLaserSpawner : UpgradeContent
{
    protected override EUpgradeType UpgradeType => EUpgradeType.Laser;

    [Header("Audio")]
    [SerializeField] private AudioClip _laserSfx;

    // 런타임 계산 값
    private double _damage;
    private float _spawnChance;

    private LeanGameObjectPool _pool;

    protected override void RefreshStats()
    {
        _damage = GetEffectValue(EUpgradeEffectType.Damage);
        _spawnChance = (float)GetEffectValue(EUpgradeEffectType.SpawnProbability);
    }

    protected override void Init()
    {
        _pool = GetComponent<LeanGameObjectPool>();
    }

    protected override void InitializeUpgradeData()
    {
        Clicker.OnClicked += OnPlayerClicked;
        base.InitializeUpgradeData();
    }

    private void OnPlayerClicked(ClickInfo clickInfo)
    {
        if (Random.value > _spawnChance) return;
        SpawnLaser();
    }

    private void SpawnLaser()
    {
        (Vector2 startPos, Vector2 endPos) = ScreenEdgeUtility.GetRandomEdgePositions();

        GameObject laserObj = _pool.Spawn(startPos, Quaternion.identity);
        ClickLaser laser = laserObj.GetComponent<ClickLaser>();
        laser.Initialize(startPos, endPos, _damage, this);
        SoundManager.Instance.PlaySfx(_laserSfx);
    }

    public void DespawnLaser(ClickLaser laser)
    {
        _pool.Despawn(laser.gameObject);
    }
}
