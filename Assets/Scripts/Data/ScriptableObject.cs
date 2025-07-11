using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public ItemType itemType; // ������ Ÿ�� (����, ����, �Ϲ� ������ ��)
    public int maxStackSize; // �ִ� ���� ���� ��
    public bool isCraftingMaterial; // ũ������ ���� ��� ��������
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
