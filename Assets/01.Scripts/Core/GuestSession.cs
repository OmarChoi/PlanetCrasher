// 로그인 없이 플레이하는 WebGL 빌드용 게스트 식별자.
// Account 생성 시 EmailSpecification 검증을 통과해야 하므로 유효한 이메일 형식을 사용한다.
// Mock 저장소들의 PlayerPrefs 키 접두사로도 사용된다.
public static class GuestSession
{
    public const string Email = "guest@planetcrasher.local";
}
