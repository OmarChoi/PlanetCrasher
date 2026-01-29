using UnityEngine;

public class SoundFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioClip _clip;

    public void Play(ClickInfo clickInfo)
    {
        float pitch = Random.Range(0.8f, 1.2f);
        SoundManager.Instance.PlaySfx(_clip, pitch);
    }
}
