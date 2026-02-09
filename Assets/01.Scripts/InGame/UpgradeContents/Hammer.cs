using UnityEngine;

public class Hammer : UpgradeContent
{
    protected override EUpgradeType UpgradeType => EUpgradeType.Hammer;
    
    [Header("Effect")]
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private AudioClip _hammerSfx;

    private double _damage;

    protected override void InitializeUpgradeData()
    {
        Clicker.OnClicked += OnPlayerClicked;
        base.InitializeUpgradeData();
    }

    private void OnPlayerClicked(ClickInfo clickInfo)
    {
        clickInfo.EffectParticle = _hitParticle;
        clickInfo.Damage = _damage;

        clickInfo.Target.OnClick(clickInfo);

        if (_hammerSfx != null)
        {
            SoundManager.Instance.PlaySfx(_hammerSfx);
        }
    }

    protected override void Cleanup()
    {
        Clicker.OnClicked -= OnPlayerClicked;
        base.Cleanup();
    }

    protected override void RefreshStats()
    {
        _damage = GetEffectValue(EUpgradeEffectType.Damage);
    }
}
