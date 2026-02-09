using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : Singleton<DamageFloaterSpawner>
{
    [SerializeField] private float _duration;
    private LeanGameObjectPool _pool;

    protected override void Initialize()
    {
        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        GameObject floaterObject = _pool.Spawn(clickInfo.Position, Quaternion.identity);
        DamageFloater damageFloater = floaterObject.GetComponent<DamageFloater>();
        damageFloater.Init();
        damageFloater.Show(clickInfo.Damage, _duration);
    }

    public void HideDamage(DamageFloater damageFloater)
    {
        _pool.Despawn(damageFloater.gameObject);
    }
}
