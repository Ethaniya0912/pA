using UnityEngine;

namespace InventorySystem // 네임스페이스 추가
{
    public class DroppedItem : MonoBehaviour
    {
        public ItemData itemData; // ItemData 필드 명확화
        public int quantity; // public 접근 제어자

        private void Start()
        {
            Debug.Log($"DroppedItem initialized on {gameObject.name}, item = {itemData?.itemName ?? "null"}, quantity = {quantity}");
        }

        public void PickUp(InventoryManager inventory)
        {
            if (inventory == null || itemData == null) return;
            if (inventory.AddItem(itemData, quantity))
            {
                Debug.Log($"Picked up {quantity} {itemData.itemName}");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning($"Failed to pick up {quantity} {itemData.itemName}: Inventory full");
            }
        }
    }
}