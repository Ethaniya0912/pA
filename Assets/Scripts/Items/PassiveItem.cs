using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    [SerializeField] private PassiveItemData itemData; // 연결된 아이템 데이터

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"PassiveItem triggered by {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            PassiveInventoryManager passiveInventory = FindObjectOfType<PassiveInventoryManager>();
            if (passiveInventory != null)
            {
                passiveInventory.AddPassiveItem(itemData);
                Destroy(gameObject);
                Debug.Log($"Acquired passive item: {itemData.itemName}");
            }
            else
            {
                Debug.LogWarning("PassiveInventoryManager not found", this);
            }
        }
    }
}