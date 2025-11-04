using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SlotUI;
using static UnityEditor.Progress;
using InventorySystem; // DroppedItem 네임스페이스 추가

public class InventoryManager : MonoBehaviour
{

    public ItemData itemApple; // Test 용

    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel; // 7x3 인벤토리 패널
    [SerializeField] private GameObject equipPanel;     // 장착 슬롯 패널
    [SerializeField] private GameObject craftingPanel;  // 작업대 패널
    [SerializeField] private GameObject quickSlotPanel; // 퀵 슬롯 패널 (선택적 토글)
    [SerializeField] private GameObject playerPanel;    // 플레이어 정보 패널 (모습, 체력, 스태미너 등)

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

    private List<InventorySlot> slots = new List<InventorySlot>();
    private List<SlotUI> slotUIs = new List<SlotUI>(); // 추가: SlotUI 관리 리스트

    private bool isInventoryOpen = false; // 인벤토리 UI 상태
    private bool isCraftingOpen = false; // 작업대 UI 상태
    private bool TabInput; // Tab 키 입력 플래그
    private bool KeyInteraction; // E 키 입력 플래그

    // Player Stats UI
    [Header("Player View")]
    [SerializeField] private Camera playerViewCamera; // PlayerViewCamera
    [SerializeField] private RawImage playerModelView; // Raw Image
    [SerializeField] private RenderTexture playerViewTexture; // Render Texture

    [Header("Stats UI")]
    [SerializeField] private TextMeshProUGUI healthText; // "HP: 100/100"
    [SerializeField] private TextMeshProUGUI staminaText; // "Stamina: 10/10"
    [SerializeField] private TextMeshProUGUI foodText; // "Food: 100/100"
    [SerializeField] private TextMeshProUGUI armorText; // "Armor: 10"
    [SerializeField] private TextMeshProUGUI strengthText; // "Strength: 5"

    [SerializeField] private PlayerStatsManager playerStatsManager; // 스탯 매니저

    // 작업대 관련
    [Header("Workbench Settings")]
    [SerializeField] private SlotUI workbenchSlotUI; // 작업대 슬롯
    [SerializeField] private Recipe workbenchRecipe; // 작업대 레시피
    [SerializeField] private GameObject workbenchPrefab; // 바닥에 설치할 작업대 프리팹
    [SerializeField] private bool autoAddWorkbench = true; // 자동 인벤토리 추가 or 드래그

    [SerializeField] private CraftingManager craftingManager;
    private bool isDragging = false;

    [SerializeField] private GameObject droppedItemPrefab; // 바닥에 드롭할 아이템 프리팹 (DroppedItem 포함)
    [SerializeField] private LayerMask groundLayer = 1 << 0; // 지면 레이어 마스크 (Default Layer = 0)
    [SerializeField] private LayerMask uiLayer = 1 << 5; // UI 레이어 마스크 (UI Layer = 5)

    [SerializeField] private Transform playerTransform; // 플레이어 Transform (아이템 드롭 위치 계산용)
    [SerializeField] private float dropDistance = 0.1f; // 아이템 드롭 거리 (캐릭터 앞)
    [SerializeField] private float dropHeight = 0.5f; // 아이템 드롭 높이 (바닥 위)

    protected virtual void Awake()
    {
        InitializeGridLayout();
        InitializeInventory();
        InitializeEquipSlots();
        InitializeQuickSlots();
        InitializeCraftingSlots();
        InitializeUI();
        InitializeWorkbenchSlot();


        // 작업대 관련
        craftingManager = FindObjectOfType<CraftingManager>();

        // 필수 컴포넌트 확인
        if (slotUIPrefab == null) Debug.LogError("slotUIPrefab not assigned", this);
        if (inventoryGrid == null) Debug.LogError("inventoryGrid not assigned", this);
        if (workbenchSlotUI == null) Debug.LogError("workbenchSlotUI not assigned", this);
        if (workbenchRecipe == null) Debug.LogError("workbenchRecipe not assigned", this);
        if (playerTransform == null) Debug.LogError("playerTransform not assigned", this);

        // workbenchSlotUI의 Button onClick 설정 (인스펙터 대신 코드로)
        Button workbenchButton = workbenchSlotUI.GetComponent<Button>();
        if (workbenchButton != null)
        {
            workbenchButton.onClick.AddListener(OnWorkbenchClick);
        }
        else
        {
            Debug.LogError("workbenchSlotUI missing Button component", this);
        }
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
        HandleStatsUI(); //플레이어 스탯 UI 갱신
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
        slots.Clear();
        slotUIs.Clear(); // 리스트 초기화

        // 인벤토리 슬롯 UI
        for (int i = 0; i < inventoryRows; i++)
        {
            for (int j = 0; j < inventoryCols; j++)
            {
                SlotUI slotUI = Instantiate(slotUIPrefab, inventoryGrid.transform);
                slotUI.transform.name = $"InventorySlot_{i}_{j}";
                //Debug.Log(slotUI);
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
        /*for (int i = 0; i < craftingSlotUIs.Length; i++)
        {
            craftingSlotUIs[i].Initialize(this, craftingSlots[i], SlotUI.SlotType.Crafting, i, Vector2Int.zero);
        }*/


        //resultSlotUI.Initialize(this, new InventorySlot(), SlotUI.SlotType.Crafting, -1, Vector2Int.zero);
        

        draggedItemIcon.enabled = false;
    }
    private void InitializeWorkbenchSlot()
    {
        if (workbenchSlotUI == null) return;

        // Workbench 슬롯용 빈 InventorySlot 생성
        InventorySlot workbenchSlotData = new InventorySlot
        {
            item = null, // 제작대는 표시용, 실제 아이템 없음
            quantity = 0
        };
        workbenchSlotUI.Initialize(this, workbenchSlotData, SlotType.Workbench, -1, new Vector2Int(-1, -1));
        slotUIs.Add(workbenchSlotUI); // 관리 리스트 추가
        Debug.Log("Initialized WorkbenchSlotUI");
    }

    // 작업대 아이콘 클릭 시 (workbenchSlotUI의 onClick 이벤트로 연결)
    public void OnWorkbenchClick()
    {
        if (workbenchRecipe == null) return;

        if (!HasMaterials(workbenchRecipe.requirements))
        {
            Debug.LogWarning("Not enough materials for Workbench");
            return; // UI 메시지 추가 추천
        }

        ConsumeMaterials(workbenchRecipe.requirements);

        // 제작대 아이템 생성
        ItemData workbenchItem = workbenchRecipe.resultItem;
        if (workbenchItem == null)
        {
            Debug.LogError("Workbench resultItem is null", this);
            return;
        }

        if (autoAddWorkbench)
        {
            bool added = AddItem(workbenchItem, 1); // 자동 인벤토리 추가
            if (added)
            {
                Debug.Log("Crafted and added Workbench to inventory");
            }
            else
            {
                Debug.LogWarning("Inventory full – cannot add Workbench");
            }
        }
        else
        {
            // 드래그 상태로 생성
            StartDragging(new InventorySlot { item = workbenchItem, quantity = 1 }, workbenchSlotUI);
            Debug.Log("Crafted Workbench and started dragging");
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

    public void EndDragging(SlotUI slotUI)
    {
        if (draggedSlot == null || draggedSlot.item == null)
        {
            Debug.LogWarning("EndDragging: draggedSlot is null or empty");
            draggedSlot = null;
            draggedSlotUI = null;
            draggedItemIcon.enabled = false;
            isDragging = false;
            return;
        }

        // UI 밖 드롭 체크
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool droppedOnUI = results.Any(r => r.gameObject.GetComponent<SlotUI>() != null);

        if (!droppedOnUI)
        {
            // 플레이어 앞에 3D 오브젝트 스폰
            if (playerTransform != null && draggedSlot.item.droppedPrefab != null)
            {
                
                DropItemToGround(draggedSlot.item, draggedSlot.quantity);
                // 인벤토리에서 아이템 제거
                RemoveItem(draggedSlot.item, draggedSlot.quantity);
            }
            else
            {
                Debug.LogWarning($"Cannot drop: playerTransform or droppedPrefab is null for {draggedSlot.item.itemName}");
            }
        }

        draggedSlot = null;
        draggedSlotUI = null;
        draggedItemIcon.enabled = false;
        isDragging = false;
        UpdateAllSlots();
    }

    private void UpdateAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.UpdateUI();
        }
    }

    private void DropItemToGround(ItemData item, int quantity)
    {
        if (playerTransform == null || item == null || item.droppedPrefab == null)
        {
            Debug.LogWarning($"DropItemToGround: Invalid parameters (playerTransform={playerTransform}, item={item?.itemName}, droppedPrefab={item?.droppedPrefab})");
            return;
        }

        Vector3 dropPos = playerTransform.position + playerTransform.forward  * 0.1f * dropDistance + Vector3.up * dropHeight;
        if (Physics.Raycast(dropPos, Vector3.down, out RaycastHit hit, 5f, LayerMask.GetMask("Ground")))
        {
            dropPos.y = hit.point.y + dropHeight;
        }

        GameObject droppedObject = Instantiate(item.droppedPrefab, dropPos, Quaternion.identity);
        droppedObject.name = $"{item.itemName}_Dropped";

        DroppedItem droppedItem = droppedObject.GetComponent<DroppedItem>();
        if (droppedItem == null)
        {
            Debug.LogWarning($"DroppedItem component missing on {droppedObject.name}. Adding it.", droppedObject);
            droppedItem = droppedObject.AddComponent<DroppedItem>();
        }

        droppedItem.itemData = item;
        droppedItem.quantity = quantity;
        Debug.Log($"Dropped {item.itemName} at {dropPos}, quantity = {quantity}, DroppedItem = {(droppedItem != null ? "set" : "null")}");

        Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(playerTransform.forward * 1f, ForceMode.Impulse);
        }

        // 원래 슬롯 비우기 (문제 해결)
        draggedSlotUI.Slot.Clear(); // 슬롯 데이터 초기화
        draggedSlotUI.UpdateUI(); // 원래 슬롯 UI 갱신 (아이콘/수량 사라짐)

        // 전체 UI 갱신 (안전성)
        UpdateUI();
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
        if (item == null)
        {
            Debug.LogError("RemoveItem: ItemData is null", this);
            return;
        }

        int remaining = quantity; // 남은 소모량

        // 1. Quick Slot에서 소모 (우선)
        for (int k = 0; k < quickSlots.Length && remaining > 0; k++)
        {
            if (quickSlots[k].item == item)
            {
                if (quickSlots[k].quantity >= remaining)
                {
                    quickSlots[k].quantity -= remaining;
                    remaining = 0;
                }
                else
                {
                    remaining -= quickSlots[k].quantity;
                    quickSlots[k].Clear(); // 슬롯 완전 비우기
                }
            }
        }

        // 2. 메인 인벤토리에서 소모
        for (int i = 0; i < inventoryRows && remaining > 0; i++)
        {
            for (int j = 0; j < inventoryCols && remaining > 0; j++)
            {
                if (inventorySlots[i, j].item == item)
                {
                    if (inventorySlots[i, j].quantity >= remaining)
                    {
                        inventorySlots[i, j].quantity -= remaining;
                        remaining = 0;
                    }
                    else
                    {
                        remaining -= inventorySlots[i, j].quantity;
                        inventorySlots[i, j].Clear();
                    }
                }
            }
        }

        if (remaining > 0)
        {
            Debug.LogWarning($"RemoveItem: Not enough {item.itemName} in inventory (needed {quantity}, removed {quantity - remaining})", this);
        }

        UpdateUI(); // UI 갱신 (아이콘/수량 표시)
        Debug.Log($"Removed {quantity - remaining} {item.itemName} from inventory");
    }

    public void DropItem(SlotUI targetSlotUI, SlotUI.SlotType targetType, int targetIndex, Vector2Int targetGridPos)
    {
        Debug.Log($"Attempting to drop item to {targetSlotUI.name} ({targetType})");
        if (draggedSlot == null) return;

        InventorySlot targetSlot = GetSlot(targetType, targetIndex, targetGridPos);
        if (targetSlot != null)
        {
            // 장착 슬롯에 맞는 아이템 타입인지 확인
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
        // 1. Quick Slot 우선 스택
        for (int k = 0; k < quickSlots.Length; k++)
        {
            if (!quickSlots[k].IsEmpty && quickSlots[k].item == item && quickSlots[k].quantity < item.maxStackSize)
            {
                quickSlots[k].AddQuantity(quantity);
                UpdateUI(); // Quick Slot UI 갱신
                Debug.Log($"Added {quantity} {item.itemName} to Quick Slot {k}");
                return true;
            }
        }
        // 2. 메인 인벤토리 스택
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
            //Debug.Log("you took apple");
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
        // E키로 작업대 UI 열기/닫기
        if (InputHandler.Instance.InteractInput)
        {
            //Debug.Log("E key pressed for interaction");
            if (!isCraftingOpen)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f))
                {
                    //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.name == "Workbench") // 설치된 작업대 오브젝트
                    {
                        // 작업대 UI 열기
                        isCraftingOpen = true;
                        craftingManager.OpenCraftingUI();
                        isInventoryOpen = true; // 인벤토리도 함께 열기
                        inventoryPanel.SetActive(true);
                        playerPanel.SetActive(false); // 작업대 열 때 플레이어 정보 패널 숨기기
                    }
                }
            }
            else if (isCraftingOpen)
            {
                // 작업대 UI 닫기
                isCraftingOpen = false;
                craftingManager.CloseCraftingUI();
                isInventoryOpen = false; // 인벤토리도 함께 닫기
                inventoryPanel.SetActive(false);
            }

            // E키로 지면 아이템 줍기
            // Raycast로 지면 아이템 히트
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray1, out RaycastHit hit1, 10.0f)) // 3m 거리
            {
                DroppedItem droppedItem = hit1.collider.GetComponent<DroppedItem>();
                if (droppedItem != null)
                {
                    // 픽업 로직은 DroppedItem.OnTriggerEnter에서 처리
                    Debug.Log("E key hit on DroppedItem");
                    droppedItem.PickUp(this);
                }
            }
            
        }
    }

    private void HandleExitCrafting()
    {
        if (isCraftingOpen)
        {
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
        if (InputHandler.Instance.isMoving)
        {
            // 인벤토리 UI 닫기
            isInventoryOpen = false;
            isCraftingOpen = false;
            craftingManager.CloseCraftingUI();
            inventoryPanel.SetActive(false);
            equipPanel.SetActive(false);
            craftingPanel.SetActive(false);
            playerPanel.SetActive(false); 
        }
    }
    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if(!isInventoryOpen)
        {
            isCraftingOpen = false;
            craftingManager.CloseCraftingUI();
            craftingPanel.SetActive(false);
        }
        Debug.Log(isInventoryOpen ? "Inventory Open" : "Inventory Close");

        inventoryPanel.SetActive(isInventoryOpen);
        equipPanel.SetActive(isInventoryOpen);
        playerPanel.SetActive(isInventoryOpen);
        if(isCraftingOpen)
        {
            playerPanel.SetActive(false);
            craftingManager.OpenCraftingUI();
            craftingPanel.SetActive(true);
        }
        else
        {
            craftingManager.CloseCraftingUI();
            craftingPanel.SetActive(false);
        }
        if (isInventoryOpen)
        {
            UpdateUI();
        }
    }

    private void HandleStatsUI()
    {
        if (playerStatsManager == null)
        {
            Debug.LogError("HandlesStatsUI: playerStatsManager is null", this);
            return;
        }
        healthText.text = $"HP: {playerStatsManager.currentHealth}/{playerStatsManager.maxHealth}";
        staminaText.text = $"Stamina: {playerStatsManager.currentStamina}/{playerStatsManager.maxStamina}";
        foodText.text = $"Food: {Math.Round(playerStatsManager.currentFood)}/{playerStatsManager.maxFood}";
        armorText.text = $"Armor: {playerStatsManager.currentArmor}";
        strengthText.text = $"Strength: {playerStatsManager.currentStrength}";
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
