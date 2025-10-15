using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemRenderer; // 아이템 스프라이트
    [SerializeField] private Collider itemCollider; // Raycast 히트용 Collider

    private ItemData item;
    private int quantity;
    private InventoryManager inventoryManager;

    public void Initialize(ItemData newItem, int qty, InventoryManager manager)
    {
        item = newItem;
        quantity = qty;
        inventoryManager = manager;

        if (itemRenderer != null && item.icon != null)
        {
            itemRenderer.sprite = item.icon;
        }
        if (itemCollider == null)
        {
            itemCollider = gameObject.AddComponent<BoxCollider>(); // 자동 Collider 추가
            itemCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어 태그 확인
        {
            // E 키 누르면 픽업
            if (Input.GetKeyDown(KeyCode.E) && inventoryManager != null)
            {
                bool pickedUp = inventoryManager.AddItem(item, quantity);
                if (pickedUp)
                {
                    Debug.Log($"Picked up {quantity} {item.itemName}");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("Inventory full, cannot pick up");
                }
            }
        }
    }
}
