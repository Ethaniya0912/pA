using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Sprite workbenchSprite; // Workbench 고정 스프라이트 (인스펙터 연결)

    private InventorySlot slot;
    private InventoryManager inventoryManager;
    private SlotType slotType;
    private int slotIndex = -1;
    private Vector2Int inventoryGridPos;

    private TooltipManager tooltipManager; // 툴팁 매니저 참조

    private void Awake()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
        if (tooltipManager == null) Debug.LogWarning("TooltipManager not found", this);
    }

    public void Initialize(InventoryManager manager, InventorySlot slotData, SlotType type, int index, Vector2Int gridPos)
    {
        if (manager == null)
        {
            Debug.LogError($"SlotUI.Initialize: InventoryManager is null in {gameObject.name}", gameObject);
            return;
        }
        if (slotData == null)
        {
            Debug.LogError($"SlotUI.Initialize: slotData is null in {gameObject.name}", gameObject);
            return;
        }
        if (itemIcon == null)
        {
            Debug.LogError($"SlotUI.Initialize: itemIcon is not assigned in {gameObject.name}", gameObject);
            return;
        }
        if (quantityText == null)
        {
            Debug.LogError($"SlotUI.Initialize: quantityText is not assigned in {gameObject.name}", gameObject);
            return;
        }

        inventoryManager = manager;
        slot = slotData;
        slotType = type;
        slotIndex = index;
        inventoryGridPos = gridPos;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (slotType == SlotType.Workbench)
        {
            // Workbench 슬롯: 항상 고정 스프라이트 표시
            if (itemIcon != null && workbenchSprite != null)
            {
                itemIcon.sprite = workbenchSprite;
                itemIcon.enabled = true; // 항상 활성화
                if (quantityText != null) quantityText.enabled = false;
                Debug.Log($"WorkbenchSlotUI: Set sprite {workbenchSprite.name}, enabled = {itemIcon.enabled}");
            }
            else
            {
                Debug.LogWarning($"WorkbenchSlotUI: itemIcon or workbenchSprite is null in {gameObject.name}", this);
            }
            return; // 일반 슬롯 로직 스킵
        }

        // 일반 슬롯
        if (slot == null || slot.IsEmpty || slot.item == null)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
                //Debug.Log($"SlotUI {gameObject.name}: Disabled itemIcon (empty slot)");
            }
            if (quantityText != null) quantityText.enabled = false;
        }
        else
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = slot.item.icon;
                itemIcon.enabled = true;
            }
            if (quantityText != null)
            {
                quantityText.text = slot.quantity.ToString();
                quantityText.enabled = slot.quantity > 1;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot == null || slot.IsEmpty || inventoryManager == null)
        {
            if (inventoryManager == null)
                Debug.LogWarning($"OnBeginDrag: inventoryManager is null in {gameObject.name}", gameObject);
            return;
        }
        inventoryManager.StartDragging(slot, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning($"OnDrag: inventoryManager is null in {gameObject.name}", gameObject);
            return;
        }
        inventoryManager.UpdateDragging(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning($"OnEndDrag: inventoryManager is null in {gameObject.name}", gameObject);
            return;
        }
        inventoryManager.EndDragging(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning($"OnDrop: inventoryManager is null in {gameObject.name}", gameObject);
            return;
        }
        inventoryManager.DropItem(this, slotType, slotIndex, inventoryGridPos);
    }

    public InventorySlot Slot => slot;
    public SlotType Type => slotType;
    public int Index => slotIndex;
    public Vector2Int GridPos => inventoryGridPos;

    public enum SlotType
    {
        Inventory, Equip, Quick, Crafting, Workbench
    }


    // Hover 시작 (IPointerEnterHandler 구현)
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        // Workbench 슬롯인 경우 고정 툴팁 표시
        if (slotType == SlotType.Workbench)
        {
            tooltipManager.ShowTooltip("Workbench", "Crafting station for advanced items", mousePos);
            Debug.Log($"OnPointerEnter: Showing Workbench tooltip for {gameObject.name} at {mousePos}");
            return; // Workbench는 slot 조건 무시
        }
        // 일반 슬롯 툴팁 처리
        if (slot == null || slot.IsEmpty || slot.item == null || tooltipManager == null)
        {
            //Debug.LogWarning($"OnPointerEnter: slot is null/empty or tooltipManager is null in {gameObject.name}", gameObject);
            //Debug.LogWarning(slot == null ? "slot is null" : slot.IsEmpty ? "slot is empty" : slot.item == null ? "slot.item is null" : "tooltipManager is null");
            return; // 유효하지 않은 경우 종료
        }

        tooltipManager.ShowTooltip(slot.item.itemName, slot.item.itemDescription, mousePos);
        //Debug.Log($"Hover on {gameObject.name}: {slot.item.itemName}"); // 디버깅 로그
    }

    // Hover 종료 (IPointerExitHandler 구현)
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }

}