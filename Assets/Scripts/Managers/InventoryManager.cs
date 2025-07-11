using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class InventorySlot
{
    public ItemData item; // 슬롯에 있는 아이템
    public int quantity; // 아이템 수량
    public bool IsEmpty => item == null; // 슬롯이 비었는지 확인

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
    [Header("슬롯 설정")]
    [SerializeField] private int inventoryRows = 3; // 인벤토리 행 수
    [SerializeField] private int inventoryCols = 7; // 인벤토리 열 수
    [SerializeField] private int quickSlotCount = 7; // 퀵 슬롯 수

    private InventorySlot[,] inventorySlots; // 7x3 인벤토리 슬롯
    private InventorySlot[] equipSlots; // 장착 슬롯 (투구, 방패 등)
    private InventorySlot[] quickSlots; // 퀵 슬롯
    private InventorySlot[] craftingSlots; // 작업대 슬롯 (크래프팅용)

    private Dictionary<ItemType, int> equipSlotIndex; // 장착 슬롯 인덱스 매핑

    // 인벤토리 설정을 위한 Config
    [SerializeField] private InventoryConfig config;

    private void Awake()
    {
        // 인벤토리 매니저 초기화
        InitializeInventory();
        InitializeEquipSlots();
        InitializeQuickSlots();
        InitializeCraftingSlots();
        // 슬롯 설정을 config에서 불러오기
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
        equipSlots = new InventorySlot[6]; // 투구, 방패, 갑옷, 하의, 신발, 화살
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
        // 작업대 슬롯 (Muck에서는 2~4개 슬롯으로 크래프팅 재료를 배치)
        craftingSlots = new InventorySlot[4]; // 예: 4개로 가정
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i] = new InventorySlot();
        }
    }

    // 아이템 추가 메서드
    public bool AddItem(ItemData item, int quantity)
    {
        // 1. 스택 가능한 경우 기존 슬롯에 추가
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
        // 2. 빈 슬롯에 추가
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
        return false; // 인벤토리 가득 참
    }

    // 장착 슬롯에 아이템 장착
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

// 퀵 슬롯에 아이템 할당
public bool AssignQuickSlot(int slotIndex, ItemData item, int quantity)
{
    if (slotIndex < 0 || slotIndex >= quickSlots.Length) return false;

    quickSlots[slotIndex].SetItem(item, quantity);
    UpdateUI();
    return true;
}

// 작업대 슬롯에 아이템 추가
public bool AddToCraftingSlot(ItemData item, int slotIndex)
{
    if (slotIndex < 0 || slotIndex >= craftingSlots.Length) return false;

    craftingSlots[slotIndex].SetItem(item, 1);
    UpdateCraftingUI();
    return true;
}

// UI 업데이트 (UI 매니저와 연동)
private void UpdateUI()
{
    // TODO: UI 슬롯에 데이터 반영 (UnityEvent 또는 UI 매니저 호출)
}

private void UpdateCraftingUI()
{
    // TODO: 작업대 UI 업데이트
}

// 작업대 크래프팅 로직
public void CraftItem()
{
    // TODO: 작업대 슬롯의 아이템을 확인하고 크래프팅 로직 구현
    // 예: 특정 조합의 아이템이 있으면 결과 아이템을 생성
}
}