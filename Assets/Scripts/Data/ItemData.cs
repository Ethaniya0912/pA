using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite icon; // 아이템 아이콘
    public ItemType itemType; // 아이템 타입 (투구, 방패, 일반 아이템 등)
    public int maxStackSize; // 최대 스택 가능 수
    public bool isCraftingMaterial; // 크래프팅 재료로 사용 가능한지
}

public enum ItemType
{
    None, Helmet, Shield, Armor, Pants, Boots, Arrow, General
}

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Config")]
public class InventoryConfig : ScriptableObject
{
    public int inventoryRows;
    public int inventoryCols;
    public int quickSlotCount;
    public int craftingSlotCount;
}
