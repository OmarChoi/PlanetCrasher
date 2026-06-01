/// <summary>
/// 백엔드(Firebase) 없이 동작하는 빌드인지를 나타내는 단일 진실 공급원(single source of truth).
/// 플랫폼 #if는 이 한 곳에만 두고, UI/게임플레이 코드는 이 플래그를 런타임으로 읽어 분기한다.
/// IsBackendless == true: 인증/저장이 모두 Mock(단일 게스트 로컬 세이브)으로 귀결되는 모드(WebGL).
/// const가 아닌 static readonly로 둬, 이 값을 읽는 런타임 분기에서 도달 불가 코드(CS0162) 경고 없이
/// 양쪽 브랜치가 모두 컴파일·타입체크되도록 한다.
/// </summary>
public static class AppEnvironment
{
#if UNITY_WEBGL && !UNITY_EDITOR
    public static readonly bool IsBackendless = true;
#else
    public static readonly bool IsBackendless = false;
#endif
}
