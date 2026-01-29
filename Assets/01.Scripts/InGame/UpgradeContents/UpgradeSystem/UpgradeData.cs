using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Item Settings")]
    public EUpgradeItem ItemType;
    public string UpgradeName;
    public Sprite Icon;
    
    [Header("Cost")]
    public int Price;
    public float PriceMultiplier = 1.15f;

    [Header("Reward")]
    public int RewardAmount;
    
    [Header("Info")]
    public EClickType ClickType;
    public int Count = 1;
    
    [TextArea]
    public string Description; // 추가 설명
}