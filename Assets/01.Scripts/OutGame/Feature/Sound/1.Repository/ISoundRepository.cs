public interface ISoundRepository
{
    void Save(SoundSaveData data);
    SoundSaveData Load();
}