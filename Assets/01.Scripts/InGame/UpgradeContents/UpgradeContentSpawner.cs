using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보유한 자동공격 콘텐츠를 해금 시점에 생성하는 InGame 팩토리.
/// UpgradeManager의 정적 이벤트만 구독하므로 OutGame Manager가 InGame을 참조하지 않는다.
/// 콘텐츠를 씬에 사전 배치하지 않으므로 "활성으로 시작해야 한다"는 묵시적 전제가 사라진다.
/// </summary>
public class UpgradeContentSpawner : MonoBehaviour
{
    [Serializable]
    private struct ContentEntry
    {
        public EUpgradeType Type;
        public UpgradeContent Prefab;
    }

    [SerializeField] private ContentEntry[] _entries;

    // 이미 생성한 콘텐츠. 업그레이드는 미보유로 되돌아가지 않으므로 1회 생성 후 유지한다.
    private readonly Dictionary<EUpgradeType, UpgradeContent> _spawned = new Dictionary<EUpgradeType, UpgradeContent>();

    private void Awake()
    {
        UpgradeManager.OnDataInitialized += SyncContents;
        UpgradeManager.OnDataChanged += SyncContents;

        // 늦게 활성화돼 이미 데이터가 준비된 경우를 대비한 즉시 동기화.
        if (UpgradeManager.Instance?.IsInitialized == true)
        {
            SyncContents();
        }
    }

    private void OnDestroy()
    {
        UpgradeManager.OnDataInitialized -= SyncContents;
        UpgradeManager.OnDataChanged -= SyncContents;
    }

    private void SyncContents()
    {
        foreach (ContentEntry entry in _entries)
        {
            if (_spawned.ContainsKey(entry.Type)) continue;
            if (!UpgradeManager.Instance.Get(entry.Type).IsOwned) continue;

            Spawn(entry);
        }
    }

    private void Spawn(ContentEntry entry)
    {
        if (entry.Prefab == null)
        {
            Debug.LogError($"[UpgradeContentSpawner] {entry.Type} 콘텐츠 프리팹이 비어 있습니다.");
            return;
        }

        // Instantiate 직후 Bind로 행성을 주입한다. _target은 첫 Start/Update에서 사용되므로
        // 이 시점(Awake 체인 종료 후, Start 이전)에 주입하면 안전하다.
        UpgradeContent content = Instantiate(entry.Prefab, transform);
        content.Bind(GameManager.Instance.CurrentPlanet);

        _spawned.Add(entry.Type, content);
    }
}
