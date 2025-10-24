using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private GameObject craftingUI; // Crafting UI �г�
    [SerializeField] private Button[] tabButtons; // �� ��ư (Basic, Tools, Station, Build)
    [SerializeField] private Transform recipeGrid; // �̸����� ������ �׸���
    [SerializeField] private GameObject recipePreviewPrefab; // �̸����� ������ (Image + onClick)
    [SerializeField] private TextMeshProUGUI hoverInfoText; // hover �� �����۸�/��� ǥ��
    [SerializeField] private List<Recipe> allRecipes = new List<Recipe>(); // ��� ������ ����Ʈ
    [SerializeField] private bool autoAddToInventory = true; // �ɼ�: �ڵ� �κ��丮 �߰� ����

    private RecipeCategory currentCategory = RecipeCategory.Basic;
    private InventoryManager inventoryManager; // ��� Ȯ�ο�
    private PlayerLocomotionManager playerLocomotionManager; // �÷��̾� ������ Ȯ�ο�

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

        craftingUI.SetActive(false); // �ʱ� ����
        SwitchTab(RecipeCategory.Basic); // �ʱ� �� ����
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

            // hover �̺�Ʈ ������Ʈ �߰� �� ������ ����
            RecipeHoverHandler hoverHandler = preview.GetComponent<RecipeHoverHandler>();
            if (hoverHandler == null)
            {
                hoverHandler = preview.AddComponent<RecipeHoverHandler>(); // ������Ʈ �߰�
            }
            hoverHandler.SetRecipe(recipe); // ���⼭ ȣ��: ������ ������ ����

            // hover �̺�Ʈ: EventTrigger.Entry�� ����� �߰�
            EventTrigger trigger = preview.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = preview.AddComponent<EventTrigger>();
            }

            // PointerEnter �̺�Ʈ �߰�
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { ShowHoverInfo(recipe); });
            trigger.triggers.Add(enterEntry);

            // PointerExit �̺�Ʈ �߰�
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { hoverInfoText.text = ""; });
            trigger.triggers.Add(exitEntry);

            // hover �̺�Ʈ (IPointerEnterHandler ���� ����)
            IPointerEnterHandler enterHandler = preview.GetComponent<IPointerEnterHandler>();
            if (enterHandler == null)
            {
                enterHandler = preview.AddComponent<RecipeHoverHandler>(); // �� ������Ʈ �߰� (�Ʒ�)
            }

            // Ŭ�� �̺�Ʈ: ���� (���� �ڵ� ����)
            Button button = preview.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => CraftItem(recipe));
            }
            else
            {
                Debug.LogWarning("Recipe preview�� Button ������Ʈ�� �����ϴ�: " + recipe.recipeName);
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

        // ������ ������ (�巡�� ����)
        if (autoAddToInventory)
        {
            bool added = inventoryManager.AddItem(recipe.resultItem, 1); // �κ��丮�� �ڵ� �߰�
            if (added)
            {
                Debug.Log("Crafted and added to inventory: " + recipe.resultItem.itemName);
            }
            else
            {
                Debug.LogWarning("Crafted but inventory full: " + recipe.resultItem.itemName);
                // Ǯ �� ��ü ����: UI �޽��� ǥ�� �Ǵ� �ӽ� ���� ����
            }
        }
        else
        {
            // �ɼ�: �巡�� ���·� ����� (���� ����)
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

