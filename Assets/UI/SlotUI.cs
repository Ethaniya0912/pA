using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Sprite workbenchSprite; // Workbench ���� ��������Ʈ (�ν����� ����)

    private InventorySlot slot;
    private InventoryManager inventoryManager;
    private SlotType slotType;
    private int slotIndex = -1;
    private Vector2Int inventoryGridPos;

    private TooltipManager tooltipManager; // ���� �Ŵ��� ����

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
            // Workbench ����: �׻� ���� ��������Ʈ ǥ��
            if (itemIcon != null && workbenchSprite != null)
            {
                itemIcon.sprite = workbenchSprite;
                itemIcon.enabled = true; // �׻� Ȱ��ȭ
                if (quantityText != null) quantityText.enabled = false;
                Debug.Log($"WorkbenchSlotUI: Set sprite {workbenchSprite.name}, enabled = {itemIcon.enabled}");
            }
            else
            {
                Debug.LogWarning($"WorkbenchSlotUI: itemIcon or workbenchSprite is null in {gameObject.name}", this);
            }
            return; // �Ϲ� ���� ���� ��ŵ
        }

        // �Ϲ� ����
        if (slot == null || slot.IsEmpty || slot.item == null)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
                Debug.Log($"SlotUI {gameObject.name}: Disabled itemIcon (empty slot)");
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


    // Hover ���� (IPointerEnterHandler ����)
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        // Workbench ������ ��� ���� ���� ǥ��
        if (slotType == SlotType.Workbench)
        {
            tooltipManager.ShowTooltip("Workbench", "Crafting station for advanced items", mousePos);
            Debug.Log($"OnPointerEnter: Showing Workbench tooltip for {gameObject.name} at {mousePos}");
            return; // Workbench�� slot ���� ����
        }
        // �Ϲ� ���� ���� ó��
        if (slot == null || slot.IsEmpty || slot.item == null || tooltipManager == null)
        {
            //Debug.LogWarning($"OnPointerEnter: slot is null/empty or tooltipManager is null in {gameObject.name}", gameObject);
            //Debug.LogWarning(slot == null ? "slot is null" : slot.IsEmpty ? "slot is empty" : slot.item == null ? "slot.item is null" : "tooltipManager is null");
            return; // ��ȿ���� ���� ��� ����
        }

        tooltipManager.ShowTooltip(slot.item.itemName, slot.item.itemDescription, mousePos);
        Debug.Log($"Hover on {gameObject.name}: {slot.item.itemName}"); // ����� �α�
    }

    // Hover ���� (IPointerExitHandler ����)
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }

}