using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDescriptionTableSO", menuName = "ScriptableObjects/EffectDescriptionTableSO")]
public class EffectDescriptionTableSO : ScriptableObject
{
    [SerializeField] private EffectDescriptionEntry[] _entries;

    private Dictionary<EUpgradeEffectType, string> _formatCache;

    public string GetFormat(EUpgradeEffectType type)
    {
        InitializeCacheIfNeeded();

        if (_formatCache.TryGetValue(type, out string format))
        {
            return format;
        }

        Debug.LogWarning($"[EffectDescriptionTableSO] No format found for {type}");
        return "{0}";
    }

    private void InitializeCacheIfNeeded()
    {
        if (_formatCache != null) return;

        _formatCache = new Dictionary<EUpgradeEffectType, string>();
        foreach (var entry in _entries)
        {
            if (_formatCache.ContainsKey(entry.Type))
            {
                Debug.LogWarning($"[EffectDescriptionTableSO] Duplicate entry for {entry.Type}");
                continue;
            }
            _formatCache[entry.Type] = entry.Format;
        }
    }

    private void OnEnable()
    {
        _formatCache = null;
    }
}
