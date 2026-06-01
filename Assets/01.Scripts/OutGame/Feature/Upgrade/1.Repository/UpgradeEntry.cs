using System;
using Firebase.Firestore;
using UnityEngine;

[Serializable]
[FirestoreData]
public class UpgradeEntry
{
    // JsonUtility(Local) 직렬화용 백킹 필드. Firestore는 아래 프로퍼티로 직렬화.
    [SerializeField] private int _typeValue;
    [SerializeField] private int _level;

    public EUpgradeType Type
    {
        get => (EUpgradeType)_typeValue;
        set => _typeValue = (int)value;
    }

    [FirestoreProperty]
    public int TypeValue
    {
        get => _typeValue;
        set => _typeValue = value;
    }

    [FirestoreProperty]
    public int Level
    {
        get => _level;
        set => _level = value;
    }

    public UpgradeEntry() { }

    public UpgradeEntry(EUpgradeType type, int level)
    {
        Type = type;
        Level = level;
    }
}