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
    private bool isCraftingOpen = false; // 작업대 UI 상태
    private bool TabInput; // Tab 키 입력 플래그
    private bool KeyInteraction; // E 키 입력 플래그

    // 작업대 관련
    [Header("Workbench Settings")]
    [SerializeField] private SlotUI workbenchSlotUI; // 작업대 슬롯
    [SerializeField] private Recipe workbenchRecipe; // 작업대 레시피
    [SerializeField] private GameObject workbenchPrefab; // 바닥에 설치할 작업대 프리팹

    [SerializeField] private CraftingManager craftingManager;
    private bool isDragging = false;

    private void Awake()
    {
        InitializeGridLayout();
        InitializeInventory();
        InitializeEquipSlots();
        InitializeQuickSlots();
        InitializeCraftingSlots();
        InitializeUI();


        // 작업대 관련
        craftingManager = FindObjectOfType<CraftingManager>();
        if (craftingManager == null) Debug.LogError("CraftingManager not found", this);
    }
    
    private void Start()
    {
        player = GetComponent<PlayerManager>();
    }

    //나중에 HandlerManager로 연동 예정
    private void Update()
    {

    }

    public void HandleAllInventorys()
    {
        AddItemApple(); //Test 용 아이템 추가
        HandleTabInput(); //Tab키 입력시 Inventory 토글
        HandleInteraction(); //E키 상호작용
        HandleExitCrafting(); //이동시 작업대 UI 닫기
        HandleExitInventory(); //이동시 인벤토리 UI 닫기
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
                slotUI.transform.name = $"InventorySlot_{i}_{j}";
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

        /*
        // 작업대 슬롯 UI
        for (int i = 0; i < craftingSlotUIs.Length; i++)
        {
            craftingSlotUIs[i].Initialize(this, craftingSlots[i], SlotUI.SlotType.Crafting, i, Vector2Int.zero);
        }
        

        resultSlotUI.Initialize(this, new InventorySlot(), SlotUI.SlotType.Crafting, -1, Vector2Int.zero);
        */

        draggedItemIcon.enabled = false;
    }

    // 작업대 아이콘 클릭 시 (workbenchSlotUI의 onClick 이벤트로 연결)
    public void OnWorkbenchClick()
    {
        if (HasMaterials(workbenchRecipe.requirements))
        {
            ConsumeMaterials(workbenchRecipe.requirements);
            StartDragging(new InventorySlot { item = workbenchRecipe.resultItem, quantity = 1 }, workbenchSlotUI);
        }
    }
    public void StartDragging(InventorySlot slot, SlotUI slotUI)
    {
        if (slot == null || slot.item == null) return;
        draggedSlot = slot;
        draggedSlotUI = slotUI;
        draggedItemIcon.enabled = true;
        draggedItemIcon.sprite = slot.item.icon;
        draggedItemIcon.rectTransform.sizeDelta = new Vector2(48, 48);
        isDragging = true;
        Debug.Log("Started dragging " + slot.item.itemName);
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
        isDragging = false;
    }


    // 재료 확인/소모 (CraftingManager와 공유)
    private bool HasMaterials(List<ItemRequirement> requirements)
    {
        foreach (var req in requirements)
        {
            int count = GetItemCount(req.item);
            if (count < req.quantity) return false;
        }
        return true;
    }

    private void ConsumeMaterials(List<ItemRequirement> requirements)
    {
        foreach (var req in requirements)
        {
            RemoveItem(req.item, req.quantity);
        }
    }

    public int GetItemCount(ItemData item)
    {
        int count = 0;
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                if (inventorySlots[i, j].item == item)
                {
                    count += inventorySlots[i, j].quantity;
                }
            }
        }
        return count;
    }

    public void RemoveItem(ItemData item, int quantity)
    {
        int remaining = quantity;
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                if (inventorySlots[i, j].item == item)
                {
                    if (inventorySlots[i, j].quantity > remaining)
                    {
                        inventorySlots[i, j].quantity -= remaining;
                        UpdateUI();
                        return;
                    }
                    else
                    {
                        remaining -= inventorySlots[i, j].quantity;
                        inventorySlots[i, j].Clear();
                        if (remaining == 0)
                        {
                            UpdateUI();
                            return;
                        }
                    }
                }
            }
        }
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
                    Debug.Log(inventorySlots[i, j].item); // item 추가되었는지 확인
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
        //foreach (var slotUI in craftingSlotUIs) slotUI.UpdateUI();
        //if(resultSlotUI !=null) resultSlotUI.UpdateUI();
    }

    private void AddItemApple() // 테스트용
    {
        if (InputHandler.Instance.LeftClickInput)
        {
            Debug.Log("you took apple");
            AddItem(itemApple,1);
        }
    }
    private void HandleTabInput()
    {
        if (InputHandler.Instance.TabInput)
        {
            ToggleInventory();
        }
    }

    // E 키 상호작용 (작업대 열기)
    private void HandleInteraction()
    {
        if (InputHandler.Instance.InteractInput)
        {
            Debug.Log("E key pressed for interaction");
            if (!isCraftingOpen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.name == "Workbench") // 설치된 작업대 오브젝트
                    {
                        // 작업대 UI 열기
                        isCraftingOpen = true;
                        craftingManager.OpenCraftingUI();
                    }
                }
            }
            else if (isCraftingOpen)
            {
                // 작업대 UI 닫기
                isCraftingOpen = false;
                craftingManager.CloseCraftingUI();
            }
        }
    }

    private void HandleExitCrafting()
    {
        if (isCraftingOpen)
        {
            if (InputHandler.Instance.moveAmount > 0)
            {
                // 작업대 UI 닫기
                isCraftingOpen = false;
                craftingManager.CloseCraftingUI();
            }
            if (InputHandler.Instance.isMoving)
            {
                // 작업대 UI 닫기
                isCraftingOpen = false;
                craftingManager.CloseCraftingUI();
            }
        }
    }
    private void HandleExitInventory()
    {
        if (isInventoryOpen)
        {
            if (InputHandler.Instance.isMoving)
            {
                // 인벤토리 UI 닫기
                isInventoryOpen = false;
                inventoryPanel.SetActive(false);
                equipPanel.SetActive(false);
                //craftingPanel.SetActive(false);
                quickSlotPanel.SetActive(false);
            }
        }
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Debug.Log(isInventoryOpen ? "Inventory Open" : "Inventory Close");
        inventoryPanel.SetActive(isInventoryOpen);
        equipPanel.SetActive(isInventoryOpen);
        //craftingPanel.SetActive(isInventoryOpen);
        quickSlotPanel.SetActive(isInventoryOpen);
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
