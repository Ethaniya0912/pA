using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemRenderer; // ������ ��������Ʈ
    [SerializeField] private Collider itemCollider; // Raycast ��Ʈ�� Collider

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
            itemCollider = gameObject.AddComponent<BoxCollider>(); // �ڵ� Collider �߰�
            itemCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾� �±� Ȯ��
        {
            // E Ű ������ �Ⱦ�
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
