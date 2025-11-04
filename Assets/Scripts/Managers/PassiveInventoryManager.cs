using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PassiveInventoryManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup passiveGrid; // 6열 그리드
    [SerializeField] private GameObject passiveSlotPrefab; // 패시브 슬롯 프리팹
    [SerializeField] private PlayerStatsManager playerStats; // 스탯 매니저

    private List<PassiveItemData> passiveItems = new List<PassiveItemData>(); // 패시브 리스트

    private void Awake()
    {
        if (passiveGrid == null) Debug.LogError("passiveGrid not assigned", this);
        if (passiveSlotPrefab == null) Debug.LogError("passiveSlotPrefab not assigned", this);
        if (playerStats == null) Debug.LogError("playerStats not assigned", this);
    }

    public void AddPassiveItem(PassiveItemData item)
    {
        if (item == null) return;

        passiveItems.Add(item);
        playerStats.ApplyPassiveItem(item); // 스탯 반영 (누적)
        UpdateUI();
        //Debug.Log($"Added passive item: {item.itemName} (rarity: {item.rarity})");
    }

    private void UpdateUI()
    {
        // 그리드 클리어
        foreach (Transform child in passiveGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in passiveItems.Distinct())
        {
            GameObject slotObj = Instantiate(passiveSlotPrefab, passiveGrid.transform);
            Image iconImage = slotObj.GetComponent<Image>();
            TextMeshProUGUI slotCount = slotObj.GetComponentInChildren<TextMeshProUGUI>();

            if (iconImage != null && item.icon != null)
            {
                iconImage.sprite = item.icon;
            }
            if (slotCount != null)
            {
                int itemCount = passiveItems.Count(i => i == item);
                slotCount.text = itemCount > 1 ? itemCount.ToString() : "";
            }
        }

        // 그리드 크기 자동 조정 (아래 확장)
        passiveGrid.GetComponent<ContentSizeFitter>().SetLayoutVertical();
    }

    private Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white; // 흰색
            case Rarity.Rare: return Color.blue; // 파란색
            case Rarity.Legend: return Color.yellow; // 노란색
            default: return Color.gray;
        }
    }

    // ... (기존 메서드, 필요 시 추가)
}