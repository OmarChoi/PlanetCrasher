public class Sound
{
    public float Volume { get; private set; }

    public Sound(float volume = 1f)
    {
        Volume = UnityEngine.Mathf.Clamp01(volume);
    }

    public void SetVolume(float volume)
    {
        Volume = UnityEngine.Mathf.Clamp01(volume);
    }
}