using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    PlayerManager player;

    public ItemData itemApple; // Test 용

    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel; // 7x3 인벤토리 패널
    [SerializeField] private GameObject equipPanel;     // 장착 슬롯 패널
    [SerializeField] private GameObject craftingPanel;  // 작업대 패널
    [SerializeField] private GameObject quickSlotPanel; // 퀵 슬롯 패널 (선택적 토글)

    [Header("GridLayoutGroup 설정")]
    [SerializeField] private GridLayoutGroup inventoryGrid;
    [SerializeField] private SlotUI slotUIPrefab;
    [SerializeField] private SlotUI[] equipSlotUIs;
    [SerializeField] private SlotUI[] quickSlotUIs;
    [SerializeField] private SlotUI[] craftingSlotUIs;
    [SerializeField] private SlotUI resultSlotUI;
    [SerializeField] private Image draggedItemIcon;
    [SerializeField] private int inventoryRows = 3;
    [SerializeField] private int inventoryCols = 7;
    [SerializeField] private int quickSlotCount = 7;

    private InventorySlot[,] inventorySlots;
    private InventorySlot[] equipSlots;
    private InventorySlot[] quickSlots;
    private InventorySlot[] craftingSlots;
    private Dictionary<ItemType, int> equipSlotIndex;
    private InventorySlot draggedSlot;
    private SlotUI draggedSlotUI;

    private bool isInventoryOpen = false; // 인벤토리 UI 상태
    private bool TabInput; // Tab 키 입력 플래그



    private void Awake()
    {
        InitializeGridLayout();
        InitializeInventory();
        InitializeEquipSlots();
        InitializeQuickSlots();
        InitializeCraftingSlots();
        InitializeUI();
    }
    
    private void Start()
    {
        player = GetComponent<PlayerManager>();
    }
    public void HandleAllInventorys()
    {
        AddItemApple(); //Test 용 아이템 추가
        HandleTabInput(); //Tab키 입력시 Inventory 토글
    }

    private void InitializeGridLayout()
    {
        inventoryGrid.padding = new RectOffset(10, 10, 10, 10);
        inventoryGrid.cellSize = new Vector2(64, 64);
        inventoryGrid.spacing = new Vector2(5, 5);
        inventoryGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        inventoryGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
        inventoryGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        inventoryGrid.constraintCount = inventoryCols;
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
        equipSlots = new InventorySlot[6];
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
        craftingSlots = new InventorySlot[4];
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i] = new InventorySlot();
        }
    }

    private void InitializeUI()
    {
        // 인벤토리 슬롯 UI
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                SlotUI slotUI = Instantiate(slotUIPrefab, inventoryGrid.transform);
                Debug.Log(slotUI);
                slotUI.Initialize(this, inventorySlots[i, j], SlotUI.SlotType.Inventory, -1, new Vector2Int(i, j));
            }
        }

        // 장착 슬롯 UI
        for (int i = 0; i < equipSlotUIs.Length; i++)
        {
            equipSlotUIs[i].Initialize(this, equipSlots[i], SlotUI.SlotType.Equip, i, Vector2Int.zero);
        }

        // 퀵 슬롯 UI
        for (int i = 0; i < quickSlotUIs.Length; i++)
        {
            quickSlotUIs[i].Initialize(this, quickSlots[i], SlotUI.SlotType.Quick, i, Vector2Int.zero);
        }

        // 작업대 슬롯 UI
        for (int i = 0; i < craftingSlotUIs.Length; i++)
        {
            craftingSlotUIs[i].Initialize(this, craftingSlots[i], SlotUI.SlotType.Crafting, i, Vector2Int.zero);
        }

        resultSlotUI.Initialize(this, new InventorySlot(), SlotUI.SlotType.Crafting, -1, Vector2Int.zero);
        draggedItemIcon.enabled = false;
    }

    public void StartDragging(InventorySlot slot, SlotUI slotUI)
    {
        draggedSlot = slot;
        draggedSlotUI = slotUI;
        draggedItemIcon.enabled = true;
        draggedItemIcon.sprite = slot.item.icon;
        draggedItemIcon.rectTransform.sizeDelta = new Vector2(48, 48);
    }

    public void UpdateDragging(Vector2 position)
    {
        draggedItemIcon.rectTransform.position = position;
    }

    public void EndDragging(SlotUI targetSlotUI)
    {
        draggedItemIcon.enabled = false;
        draggedSlot = null;
        draggedSlotUI = null;
    }

    public void DropItem(SlotUI targetSlotUI, SlotUI.SlotType targetType, int targetIndex, Vector2Int targetGridPos)
    {
        if (draggedSlot == null) return;

        InventorySlot targetSlot = GetSlot(targetType, targetIndex, targetGridPos);
        if (targetSlot != null)
        {
            if (targetType == SlotUI.SlotType.Equip)
            {
                ItemType requiredType = GetEquipSlotType(targetIndex);
                if (draggedSlot.item.itemType != requiredType) return;
            }

            var tempItem = targetSlot.item;
            var tempQuantity = targetSlot.quantity;
            targetSlot.SetItem(draggedSlot.item, draggedSlot.quantity);
            draggedSlotUI.Slot.SetItem(tempItem, tempQuantity);

            targetSlotUI.UpdateUI();
            draggedSlotUI.UpdateUI();
        }
        // 디버깅
        if (targetType == SlotUI.SlotType.Equip && (targetIndex < 0 || targetIndex >= equipSlots.Length))
        {
            Debug.LogError($"DropItem: Invalid targetIndex {targetIndex}", this);
            return;
        }
    }

    private InventorySlot GetSlot(SlotUI.SlotType type, int index, Vector2Int gridPos)
    {
        switch (type)
        {
            case SlotUI.SlotType.Inventory:
                return inventorySlots[gridPos.x, gridPos.y];
            case SlotUI.SlotType.Equip:
                return equipSlots[index];
            case SlotUI.SlotType.Quick:
                return quickSlots[index];
            case SlotUI.SlotType.Crafting:
                return craftingSlots[index];
            default:
                return null;
        }
    }

    private ItemType GetEquipSlotType(int index)
    {
        var pair = equipSlotIndex.FirstOrDefault(x => x.Value == index);
        if (pair.Equals(default(KeyValuePair<ItemType, int>)))
        {
            Debug.LogError($"GetEquipSlotType: No ItemType found for index {index}", this);
            return ItemType.General; // 기본값 반환
        }
        return pair.Key;
    }

    public bool AddItem(ItemData item, int quantity)
    {
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
        return false;
    }

    private void UpdateUI()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            SlotUI slotUI = child.GetComponent<SlotUI>();
            if (slotUI != null) slotUI.UpdateUI();
        }
        foreach (var slotUI in equipSlotUIs) slotUI.UpdateUI();
        foreach (var slotUI in quickSlotUIs) slotUI.UpdateUI();
        foreach (var slotUI in craftingSlotUIs) slotUI.UpdateUI();
        if(resultSlotUI !=null) resultSlotUI.UpdateUI();
    }

    private void AddItemApple()
    {
        if (InputHandler.Instance.LeftClickInput)
        {
            Debug.Log("you took apple");
            AddItem(itemApple,1);
            Debug.Log(inventorySlots);
        }
    }
    private void HandleTabInput()
    {
        if (InputHandler.Instance.TabInput)
        {
            ToggleInventory();
        }
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Debug.Log(isInventoryOpen ? "Inventory Open" : "Inventory Close");
        inventoryPanel.SetActive(isInventoryOpen);
        equipPanel.SetActive(isInventoryOpen);
        craftingPanel.SetActive(isInventoryOpen);
        //quickSlotPanel.SetActive(isInventoryOpen);
        if (isInventoryOpen)
        {
            UpdateUI();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public Sprite itemIcon;
    public int quantity;
    public bool IsEmpty => item == null;

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
