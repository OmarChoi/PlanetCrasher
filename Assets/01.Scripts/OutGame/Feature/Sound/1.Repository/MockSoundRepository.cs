// 로그인이 필요 없는 WebGL 빌드용 Mock 저장소.
// LocalSoundRepository의 PlayerPrefs 로직을 고정 게스트 키로 재사용한다.
public class MockSoundRepository : LocalSoundRepository
{
    public MockSoundRepository() : base(GuestSession.Email)
    {
    }
}
