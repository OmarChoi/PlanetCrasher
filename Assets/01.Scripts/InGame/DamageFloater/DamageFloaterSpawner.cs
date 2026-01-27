using System.Collections;
using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }
    [SerializeField] private LeanGameObjectPool _pool;
    [SerializeField] private float _duration;
    
    private void Awake()
    {
        Instance = this;
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
