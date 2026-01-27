using UnityEngine;

public class ParticleEffect : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.EffectParticle == null) return;
        clickInfo.EffectParticle.transform.position = clickInfo.Position;
        clickInfo.EffectParticle.Play();
    }
}
