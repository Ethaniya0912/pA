using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public ItemType itemType; // ������ Ÿ�� (����, ����, �Ϲ� ������ ��)
    public int maxStackSize; // �ִ� ���� ���� ��
    public bool isCraftingMaterial; // ũ������ ���� ��� ��������
    public bool isCraftable = false; // ���� ���� ����
    public RecipeCategory category = RecipeCategory.Basic; // ������ ī�װ�
}

public enum ItemType
{
    None, Helmet, Shield, Armor, Pants, Boots, Arrow, General, Workbench
}

public enum RecipeCategory
{
    Basic, Tools, Station, Build
}

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Config")]
public class InventoryConfig : ScriptableObject
{
    public int inventoryRows;
    public int inventoryCols;
    public int quickSlotCount;
    public int craftingSlotCount;
}
