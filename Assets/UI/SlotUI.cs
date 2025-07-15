using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    private InventorySlot slot;
    private InventoryManager inventoryManager;
    private SlotType slotType;
    private int slotIndex = -1;
    private Vector2Int inventoryGridPos;

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
    if (itemIcon == null || quantityText == null)
    {
        Debug.LogError($"UpdateUI: itemIcon or quantityText is null in {gameObject.name}", gameObject);
        return;
    }

    if (slot == null || slot.IsEmpty)
    {
        itemIcon.enabled = false; // 아이콘 비활성화
        quantityText.text = "";
    }
    else
    {
        if (slot.item == null || slot.item.icon == null)
        {
            Debug.LogWarning($"UpdateUI: slot.item or slot.item.icon is null in {gameObject.name}", gameObject);
            itemIcon.enabled = false; // 아이콘 비활성화
            quantityText.text = "";
            return;
        }
        itemIcon.enabled = true; // 아이콘 활성화
        itemIcon.sprite = slot.item.icon; // 아이콘 스프라이트 갱신
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
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
        Inventory, Equip, Quick, Crafting
    }
}