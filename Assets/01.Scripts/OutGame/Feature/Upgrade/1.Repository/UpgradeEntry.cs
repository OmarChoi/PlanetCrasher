using System;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class UpgradeEntry
{
    public EUpgradeType Type
    {
        get => (EUpgradeType)TypeValue;
        set => TypeValue = (int)value;
    }

    [FirestoreProperty]
    public int TypeValue { get; set; }

    [FirestoreProperty]
    public int Level { get; set; }

    public UpgradeEntry() { }

    public UpgradeEntry(EUpgradeType type, int level)
    {
        Type = type;
        Level = level;
    }
}