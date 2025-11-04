using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Item", menuName = "Inventory/Passive Item")]
public class PassiveItemData : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite icon; // 아이콘 (48x48)
    public GameObject droppedPrefab; // 3D 드롭 프리팹
    public Rarity rarity; // 등급
    public PowerupType powerupType; // 효과 타입
    public float powerupValue; // 효과 값 (중복 누적)
    public string description; // 툴팁 설명
}

public enum Rarity
{
    Common, // 흰색
    Rare, // 파란색
    Legend // 노란색
}

public enum PowerupType
{
    None, 
    MaxHP, 
    MaxStamina, 
    HealthRegen,
    StaminaRegen,
    JumpHeight, 
    MoveSpeed, 
    Strength, 
    ArrowDamage, 
    AttackSpeed, 
    Lifesteal, 
    CritChance, 
    CritLifesteal, 
    DropRate,
    Armor,
    Shield
}