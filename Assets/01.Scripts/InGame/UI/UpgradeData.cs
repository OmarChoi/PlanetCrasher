using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string UpgradeName;
    public Sprite Icon;
    
    [Header("Cost")]
    public int Price;

    [Header("Reward")]
    public int RewardAmount;
    
    [Header("Info")]
    public EClickType Type;
    public int Count = 1;
    
    [TextArea]
    public string Description; // 추가 설명
}