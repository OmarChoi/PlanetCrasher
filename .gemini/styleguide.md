# PlanetCrasher 코드 리뷰 지침

이 문서는 PlanetCrasher 프로젝트의 코드 리뷰를 위한 지침입니다. 모든 PR은 아래 규칙을 준수해야 합니다.

---

## 0. 코드 리뷰 봇 지침 (Gemini Code Assistant)

* **리뷰 언어:** 모든 코드 리뷰 요약 및 코멘트를 **한국어(Korean)**로 작성해야 합니다.
* **디자인 원칙 검토:** 모든 PR에 대해 **SOLID 원칙**과 **디미터의 법칙(Law of Demeter)** 위반 여부를 검토합니다.
* **함수:** 함수는 한 가지 일만을 해야 합니다.
* **아키텍처 준수:** 프로젝트 고유의 계층 구조와 패턴 준수 여부를 반드시 확인합니다.

---

## 1. 아키텍처 규칙

### 1.1. InGame / OutGame 분리
* **InGame** (`InGame/`): 게임 메커니즘 (클릭, 행성, 피드백, 업그레이드 콘텐츠)
* **OutGame** (`OutGame/Feature/`): 비즈니스 로직 및 데이터 관리 (화폐, 업그레이드, 사운드, 계정)
* InGame에서 OutGame Manager의 공개 API를 호출하는 것은 허용됩니다. (예: `CurrencyManager.Instance.Add()`)
* OutGame에서 InGame 클래스를 직접 참조하는 것은 **금지**됩니다. 이벤트를 통해 소통해야 합니다.

### 1.2. OutGame 3계층 구조
모든 OutGame Feature는 반드시 다음 3계층을 따라야 합니다:
```
OutGame/Feature/[FeatureName]/
├── 1.Repository/   # 저장/로드 (인터페이스 + Local/Firebase 구현체)
├── 2.Domain/       # 비즈니스 로직 (Enum, Value Object, Domain Object)
└── 3.Manager/      # Singleton Manager (퍼블릭 API, 이벤트 발행)
```

**검토 포인트:**
* Repository는 반드시 인터페이스(`I[Name]Repository`)를 정의하고 구현체가 이를 구현해야 합니다.
* Domain 객체는 Unity에 의존하지 않아야 합니다. (MonoBehaviour 상속 금지)
* Manager는 Singleton이며, 정적 이벤트를 통해 상태 변경을 알려야 합니다.

### 1.3. 이벤트 기반 통신
계층 간 통신은 정적 이벤트를 사용합니다:
```csharp
// 올바른 예: 이벤트 구독으로 느슨한 결합
UpgradeManager.OnDataChanged += RefreshStats;

// 잘못된 예: 직접 참조로 강한 결합
_upgradePanel.RefreshAllItems();
```

**검토 포인트:**
* 이벤트 구독(`+=`)은 반드시 대응하는 구독 해제(`-=`)가 있어야 합니다.
* 구독은 `Awake()`/`OnEnable()`에서, 해제는 `OnDisable()`/`OnDestroy()`에서 수행합니다.

### 1.4. MetaData vs Runtime 데이터 분리
* **MetaData**: ScriptableObject로 관리하는 기획 데이터 (변경 불가)
  - 예: `UpgradeMetaData` (BaseCost, MaxLevel, Effects[])
* **Runtime 데이터**: 게임 진행 중 변하는 데이터
  - 예: `Upgrade.Level`, `Currency` 잔액

**검토 포인트:**
* ScriptableObject의 필드를 런타임에 수정하는 코드가 있으면 지적합니다.
* 기획 데이터와 런타임 데이터가 한 클래스에 혼재되어 있으면 분리를 권고합니다.

---

## 2. 디자인 원칙

### 2.1. SOLID 원칙

| 원칙 | 핵심 요약 | 프로젝트 적용 예시 |
| :--- | :--- | :--- |
| **SRP** | 클래스는 단 하나의 변경 이유만 가져야 합니다 | `Planet`은 체력/데미지만 담당, 화폐는 `CurrencyManager`에 위임 |
| **OCP** | 확장에 열리고 수정에 닫혀야 합니다 | 새 피드백 추가 시 `IFeedback` 구현만 하면 되고 `Planet` 수정 불필요 |
| **LSP** | 하위 타입으로 치환해도 동작해야 합니다 | 모든 `UpgradeContent` 서브클래스는 `RefreshStats()` 계약을 이행 |
| **ISP** | 사용하지 않는 인터페이스에 의존하지 않습니다 | `IClickable`은 `OnClick()`만, `IFeedback`은 `Play()`만 정의 |
| **DIP** | 추상화에 의존해야 합니다 | Manager는 `ICurrencyRepository` 인터페이스에 의존, 구현체(Local/Firebase)에 의존하지 않음 |

### 2.2. 디미터의 법칙 (Law of Demeter)
```csharp
// 위반 예시 (기차 참사)
UpgradeManager.Instance.Get(type).MetaData.Effects[0].BaseValue;

// 준수 예시
var upgrade = UpgradeManager.Instance.Get(type);
var damage = upgrade.GetEffectValue(EUpgradeEffectType.Damage);
```

---

## 3. 명명 규칙

### 3.1. 필수 네이밍 패턴

| 대상 | 규칙 | 예시 |
| :--- | :--- | :--- |
| Private 인스턴스 필드 | `_camelCase` | `_health`, `_damage` |
| SerializeField | `_camelCase` | `[SerializeField] private float _interval` |
| Public 속성 | `PascalCase` | `public double ManualDamage => _manualDamage` |
| 메서드 | `PascalCase` | `ChangePlanet()`, `TryLevelUp()` |
| 코루틴 | `메서드명_Coroutine` | `Spawn_Coroutine()` |
| Interface | `I` + PascalCase | `IClickable`, `IFeedback` |
| Enum | `E` + PascalCase | `EClickType`, `EUpgradeType` |
| UI 클래스 | `UI_` + 기능명 | `UI_UpgradePanel`, `UI_Gold` |
| ScriptableObject | 기능명 + `SO` | `UpgradeSpecTableSO` |

### 3.2. 클래스 멤버 순서
```csharp
public class Example : MonoBehaviour
{
    // 1. Static 멤버 (Instance, Events)
    public static Example Instance { get; private set; }
    public static event Action OnDataChanged;

    // 2. SerializeField 필드
    [SerializeField] private float _speed;

    // 3. Private 필드
    private double _health;

    // 4. Properties
    public double Health => _health;

    // 5. Unity 라이프사이클 (Awake → Start → Update → OnDisable)
    private void Awake() { }
    private void Update() { }
    private void OnDisable() { }

    // 6. Public 메서드
    public void TakeDamage(double amount) { }

    // 7. Private 메서드
    private void UpdateVisual() { }

    // 8. Coroutines
    private IEnumerator Spawn_Coroutine() { }
}
```

**검토 포인트:**
* 멤버 순서가 위 규칙을 따르지 않으면 지적합니다.
* 접근 제어자가 누락된 멤버가 있으면 지적합니다.

---

## 4. 코드 스타일 규칙

### 4.1. 필수 규칙
* `readonly` 적극 활용. Value Object는 `readonly struct`로 선언합니다.
* 접근 제어자를 항상 명시합니다. (`public`, `private`, `protected`)
* 이벤트는 정적으로 선언합니다: `public static event Action OnDataChanged;`
* `var`는 타입이 명확히 유추되는 경우에만 사용합니다.
* 문자열 연결은 보간(`$"{}"`)을 사용하고, 루프 내 대량 연결은 `StringBuilder`를 사용합니다.

### 4.3. 에러 처리 및 결과 반환 패턴
상황에 따라 적절한 반환 패턴을 선택합니다:

| 상황 | 패턴 | 예시 |
| :--- | :--- | :--- |
| 단순 성공/실패 (에러 메시지 불필요) | `bool TryXxx()` | `TrySpend()`, `TryLevelUp()` |
| 실패 원인을 호출자에게 전달해야 할 때 | Result 구조체 반환 | `UniTask<AccountResult> TryLogin()` |
| 입력값 검증 | `ValidationResult` + 팩토리 메서드 | `ISpecification<T>.IsSatisfiedBy()` |

```csharp
// 1. bool TryXxx - 단순 성공/실패
public bool TrySpend(ECurrencyType type, Currency amount)
{
    if (_currencies[(int)type] < amount) return false;
    _currencies[(int)type] -= amount;
    OnDataChanged?.Invoke();
    return true;
}

// 2. Result 구조체 - 에러 메시지가 필요한 async 작업
public async UniTask<AccountResult> TryLogin(string email, string password)
{
    var result = _passwordSpecification.IsSatisfiedBy(password);
    if (!result.IsValid)
        return new AccountResult(success: false, errorMessage: result.ErrorMessage, account: null);
    // ...
}

// 3. ValidationResult - Specification 검증
public ValidationResult IsSatisfiedBy(string input)
{
    if (string.IsNullOrEmpty(input))
        return ValidationResult.Failure("이메일을 입력해주세요.");
    return ValidationResult.Success();
}
```

**검토 포인트:**
* 에러 메시지가 필요 없는 곳에서 Result 구조체를 쓰고 있으면 `bool TryXxx()`로 단순화를 권고합니다.
* 호출자가 실패 원인을 UI에 표시해야 하는데 `bool`만 반환하고 있으면 Result 구조체 도입을 권고합니다.
* `TryXxx` 메서드는 예외를 던지지 않아야 합니다. 실패 시 `false` 또는 실패 Result를 반환합니다.

### 4.4. UniTask vs Coroutine 사용 기준

| 용도 | 사용할 것 | 예시 |
| :--- | :--- | :--- |
| 데이터 로드/저장, 네트워크 I/O | **UniTask** (`async`/`await`) | `Repository.Load()`, `Repository.Save()` |
| 타이밍 기반 효과, 반복 스폰 루프 | **Coroutine** (`IEnumerator`) | `PlaySfx_Coroutine()`, `Spawn_Coroutine()` |

```csharp
// UniTask: 비동기 I/O 작업
private async UniTaskVoid LoadData()
{
    CurrencySaveData data = await _repository.Load();
    // ...
}

// Awake에서 fire-and-forget 호출 시 .Forget() 사용
private void Awake()
{
    LoadData().Forget();
}

// Coroutine: 타이밍 기반 게임플레이 효과
private IEnumerator PlaySfx_Coroutine(AudioSource source, float duration)
{
    yield return new WaitForSeconds(duration);
    LeanPool.Despawn(source);
}
```

**검토 포인트:**
* I/O 작업(Firebase, 파일)에 Coroutine을 사용하고 있으면 UniTask로 변경을 권고합니다.
* 타이밍/애니메이션 효과에 UniTask를 사용하고 있으면 Coroutine으로 변경을 권고합니다.
* `async UniTaskVoid`를 Awake/Start에서 호출할 때 `.Forget()`을 빠뜨리면 경고가 발생하므로 확인합니다.

### 4.2. 레이아웃
* 들여쓰기: **4개의 공백** (탭 사용 금지)
* 중괄호: **Allman 스타일** (별도 줄에 배치)
* 한 줄에 하나의 문장, 하나의 선언
* 메서드 간 빈 줄 추가
* XML 주석(`<summary>` 등)은 작성하지 않습니다. 메서드명과 매개변수명이 충분히 설명적이어야 합니다.

---

## 5. 패턴별 리뷰 체크리스트

### 5.1. 새 업그레이드 추가 시
- [ ] `EUpgradeType`에 새 타입이 추가되었는가
- [ ] `UpgradeSpecTableSO`에 메타데이터가 설정되었는가 (BaseCost, Effects[], CostIncreaseType)
- [ ] `UpgradeContent`를 상속한 콘텐츠 스크립트가 `InGame/UpgradeContents/`에 있는가
- [ ] `RefreshStats()`에서 `GetEffectValue()`로 스탯을 갱신하는가
- [ ] 필요한 경우 `EUpgradeEffectType`에 새 효과 타입이 추가되었는가
- [ ] 투사체가 필요하면 `Items/`에 작성되었고 LeanPool을 사용하는가

### 5.2. 새 피드백 추가 시
- [ ] `IFeedback` 인터페이스를 구현하는가
- [ ] `Play(ClickInfo clickInfo)` 메서드가 올바르게 구현되었는가
- [ ] Planet/Asteroid의 자식 오브젝트로 배치되는가

### 5.3. 새 OutGame Feature 추가 시
- [ ] `OutGame/Feature/[FeatureName]/` 폴더 구조를 따르는가 (1.Repository, 2.Domain, 3.Manager)
- [ ] Repository 인터페이스(`I[Name]Repository`)가 정의되었는가
- [ ] Local/Firebase 두 구현체가 있는가
- [ ] Manager가 Singleton이며 정적 이벤트를 발행하는가
- [ ] Domain 객체가 Unity 의존성 없이 순수하게 작성되었는가

### 5.4. 새 UI 추가 시
- [ ] `UI_기능명` 네이밍을 따르는가
- [ ] Manager의 정적 이벤트를 구독하여 데이터를 갱신하는가
- [ ] `OnDisable()`에서 이벤트 구독을 해제하는가

---

## 6. 성능 리뷰 포인트

* `Update()`에서 무거운 연산이나 할당이 발생하지 않는지 확인합니다.
* 자주 생성/파괴되는 오브젝트(Bullet, Missile, AudioSource 등)는 **LeanPool**을 사용해야 합니다.
* `GetComponent<T>()`는 `Awake()`에서 캐싱하고, 매 프레임 호출하지 않습니다.
* 매 프레임 문자열 할당(`ToString()`, 문자열 연결)이 발생하지 않는지 확인합니다.

---

코드 요약과 리뷰는 한국어로 해주세요.
