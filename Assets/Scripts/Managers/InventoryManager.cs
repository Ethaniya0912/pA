using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class InventorySlot
{
    public ItemData item; // ���Կ� �ִ� ������
    public int quantity; // ������ ����
    public bool IsEmpty => item == null; // ������ ������� Ȯ��

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public void SetItem(ItemData newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }
}

public class InventoryManager : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private int inventoryRows = 3; // �κ��丮 �� ��
    [SerializeField] private int inventoryCols = 7; // �κ��丮 �� ��
    [SerializeField] private int quickSlotCount = 7; // �� ���� ��

    private InventorySlot[,] inventorySlots; // 7x3 �κ��丮 ����
    private InventorySlot[] equipSlots; // ���� ���� (����, ���� ��)
    private InventorySlot[] quickSlots; // �� ����
    private InventorySlot[] craftingSlots; // �۾��� ���� (ũ�����ÿ�)

    private Dictionary<ItemType, int> equipSlotIndex; // ���� ���� �ε��� ����

    // �κ��丮 ������ ���� Config
    [SerializeField] private InventoryConfig config;

    private void Awake()
    {
        // �κ��丮 �Ŵ��� �ʱ�ȭ
        InitializeInventory();
        InitializeEquipSlots();
        InitializeQuickSlots();
        InitializeCraftingSlots();
        // ���� ������ config���� �ҷ�����
        inventoryRows = config.inventoryRows;
        inventoryCols = config.inventoryCols;
        quickSlotCount = config.quickSlotCount;
    }

    private void InitializeInventory()
    {
        inventorySlots = new InventorySlot[inventoryRows, inventoryCols];
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                inventorySlots[i, j] = new InventorySlot();
            }
        }
    }

    private void InitializeEquipSlots()
    {
        equipSlots = new InventorySlot[6]; // ����, ����, ����, ����, �Ź�, ȭ��
        equipSlotIndex = new Dictionary<ItemType, int>
        {
            { ItemType.Helmet, 0 },
            { ItemType.Shield, 1 },
            { ItemType.Armor, 2 },
            { ItemType.Pants, 3 },
            { ItemType.Boots, 4 },
            { ItemType.Arrow, 5 }
        };
        for (int i = 0; i < equipSlots.Length; i++)
        {
            equipSlots[i] = new InventorySlot();
        }
    }

    private void InitializeQuickSlots()
    {
        quickSlots = new InventorySlot[quickSlotCount];
        for (int i = 0; i < quickSlotCount; i++)
        {
            quickSlots[i] = new InventorySlot();
        }
    }

    private void InitializeCraftingSlots()
    {
        // �۾��� ���� (Muck������ 2~4�� �������� ũ������ ��Ḧ ��ġ)
        craftingSlots = new InventorySlot[4]; // ��: 4���� ����
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i] = new InventorySlot();
        }
    }

    // ������ �߰� �޼���
    public bool AddItem(ItemData item, int quantity)
    {
        // 1. ���� ������ ��� ���� ���Կ� �߰�
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                if (!inventorySlots[i, j].IsEmpty && inventorySlots[i, j].item == item && inventorySlots[i, j].quantity < item.maxStackSize)
                {
                    inventorySlots[i, j].AddQuantity(quantity);
                    UpdateUI();
                    return true;
                }
            }
        }
        // 2. �� ���Կ� �߰�
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                if (inventorySlots[i, j].IsEmpty)
                {
                    inventorySlots[i, j].SetItem(item, quantity);
                    UpdateUI();
                    return true;
                }
            }
        }
        return false; // �κ��丮 ���� ��
    }

    // ���� ���Կ� ������ ����
    public bool EquipItem(ItemData item)
{
    if (item.itemType == ItemType.General) return false;

    int index = equipSlotIndex[item.itemType];
    if (equipSlots[index].IsEmpty)
    {
        equipSlots[index].SetItem(item, 1);
        UpdateUI();
        return true;
    }
    return false;
}

// �� ���Կ� ������ �Ҵ�
public bool AssignQuickSlot(int slotIndex, ItemData item, int quantity)
{
    if (slotIndex < 0 || slotIndex >= quickSlots.Length) return false;

    quickSlots[slotIndex].SetItem(item, quantity);
    UpdateUI();
    return true;
}

// �۾��� ���Կ� ������ �߰�
public bool AddToCraftingSlot(ItemData item, int slotIndex)
{
    if (slotIndex < 0 || slotIndex >= craftingSlots.Length) return false;

    craftingSlots[slotIndex].SetItem(item, 1);
    UpdateCraftingUI();
    return true;
}

// UI ������Ʈ (UI �Ŵ����� ����)
private void UpdateUI()
{
    // TODO: UI ���Կ� ������ �ݿ� (UnityEvent �Ǵ� UI �Ŵ��� ȣ��)
}

private void UpdateCraftingUI()
{
    // TODO: �۾��� UI ������Ʈ
}

// �۾��� ũ������ ����
public void CraftItem()
{
    // TODO: �۾��� ������ �������� Ȯ���ϰ� ũ������ ���� ����
    // ��: Ư�� ������ �������� ������ ��� �������� ����
}
}