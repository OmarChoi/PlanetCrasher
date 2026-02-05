using UnityEngine;

public struct ClickInfo
{
    public EClickType Type;
    public IClickable Target;
    public double Damage;
    public Vector2 Position;
    public ParticleSystem EffectParticle;
}