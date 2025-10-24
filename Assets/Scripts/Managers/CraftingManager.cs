using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private GameObject craftingUI; // Crafting UI 패널
    [SerializeField] private Button[] tabButtons; // 탭 버튼 (Basic, Tools, Station, Build)
    [SerializeField] private Transform recipeGrid; // 미리보기 아이콘 그리드
    [SerializeField] private GameObject recipePreviewPrefab; // 미리보기 프리팹 (Image + onClick)
    [SerializeField] private TextMeshProUGUI hoverInfoText; // hover 시 아이템명/재료 표시
    [SerializeField] private List<Recipe> allRecipes = new List<Recipe>(); // 모든 레시피 리스트
    [SerializeField] private bool autoAddToInventory = true; // 옵션: 자동 인벤토리 추가 여부

    private RecipeCategory currentCategory = RecipeCategory.Basic;
    private InventoryManager inventoryManager; // 재료 확인용
    private PlayerLocomotionManager playerLocomotionManager; // 플레이어 움직임 확인용

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null) Debug.LogError("CraftingManager: InventoryManager not found", this);

        playerLocomotionManager = FindObjectOfType<PlayerLocomotionManager>();
        if (playerLocomotionManager == null) Debug.LogError("CraftingManager: PlayerLocomotionManager not found", this);

        for (int i = 0; i < tabButtons.Length; i++)
        {
            int categoryIndex = i;
            tabButtons[i].onClick.AddListener(() => SwitchTab((RecipeCategory)categoryIndex));
        }

        craftingUI.SetActive(false); // 초기 숨김
        SwitchTab(RecipeCategory.Basic); // 초기 탭 설정
    }

    public void OpenCraftingUI()
    {
        //Debug.Log("Open Crafting UI");
        craftingUI.SetActive(true);
        SwitchTab(currentCategory);
    }

    public void CloseCraftingUI()
    {
        craftingUI.SetActive(false);
    }

    private void SwitchTab(RecipeCategory category)
    {
        currentCategory = category;
        ClearGrid();

        var categoryRecipes = allRecipes.Where(r => r.category == category).ToList();
        foreach (var recipe in categoryRecipes)
        {
            GameObject preview = Instantiate(recipePreviewPrefab, recipeGrid);
            Image img = preview.GetComponent<Image>();
            img.sprite = recipe.previewIcon;

            // hover 이벤트 컴포넌트 추가 및 레시피 설정
            RecipeHoverHandler hoverHandler = preview.GetComponent<RecipeHoverHandler>();
            if (hoverHandler == null)
            {
                hoverHandler = preview.AddComponent<RecipeHoverHandler>(); // 컴포넌트 추가
            }
            hoverHandler.SetRecipe(recipe); // 여기서 호출: 레시피 데이터 전달

            // hover 이벤트: EventTrigger.Entry를 사용해 추가
            EventTrigger trigger = preview.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = preview.AddComponent<EventTrigger>();
            }

            // PointerEnter 이벤트 추가
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { ShowHoverInfo(recipe); });
            trigger.triggers.Add(enterEntry);

            // PointerExit 이벤트 추가
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { hoverInfoText.text = ""; });
            trigger.triggers.Add(exitEntry);

            // hover 이벤트 (IPointerEnterHandler 구현 가정)
            IPointerEnterHandler enterHandler = preview.GetComponent<IPointerEnterHandler>();
            if (enterHandler == null)
            {
                enterHandler = preview.AddComponent<RecipeHoverHandler>(); // 새 컴포넌트 추가 (아래)
            }

            // 클릭 이벤트: 제작 (기존 코드 유지)
            Button button = preview.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => CraftItem(recipe));
            }
            else
            {
                Debug.LogWarning("Recipe preview에 Button 컴포넌트가 없습니다: " + recipe.recipeName);
            }
        }
    }

    private void ClearGrid()
    {
        foreach (Transform child in recipeGrid)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowHoverInfo(Recipe recipe)
    {
        string info = $"Name: {recipe.recipeName}\nRequirements:\n";
        foreach (var req in recipe.requirements)
        {
            info += $"- {req.item.itemName} x {req.quantity}\n";
        }
        hoverInfoText.text = info;
    }

    private void CraftItem(Recipe recipe)
    {
        if (recipe == null || inventoryManager == null)
        {
            Debug.LogError("CraftItem: Recipe or InventoryManager is null", this);
            return;
        }

        if (!HasMaterials(recipe.requirements))
        {
            Debug.LogWarning("Not enough materials for " + recipe.recipeName);
            return;
        }

        ConsumeMaterials(recipe.requirements);

        // 아이템 생성만 (드래그 없이)
        if (autoAddToInventory)
        {
            bool added = inventoryManager.AddItem(recipe.resultItem, 1); // 인벤토리에 자동 추가
            if (added)
            {
                Debug.Log("Crafted and added to inventory: " + recipe.resultItem.itemName);
            }
            else
            {
                Debug.LogWarning("Crafted but inventory full: " + recipe.resultItem.itemName);
                // 풀 시 대체 로직: UI 메시지 표시 또는 임시 슬롯 생성
            }
        }
        else
        {
            // 옵션: 드래그 상태로 만들기 (기존 로직)
            inventoryManager.StartDragging(new InventorySlot { item = recipe.resultItem, quantity = 1 }, null);
            Debug.Log("Crafted and started dragging: " + recipe.resultItem.itemName);
        }
    }

    private bool HasMaterials(List<ItemRequirement> requirements)
    {
        foreach (var req in requirements)
        {
            int count = inventoryManager.GetItemCount(req.item);
            if (count < req.quantity)
            {
                return false;
            }
        }
        return true;
    }

    private void ConsumeMaterials(List<ItemRequirement> requirements)
    {
        foreach (var req in requirements)
        {
            inventoryManager.RemoveItem(req.item, req.quantity);
        }
    }

}

