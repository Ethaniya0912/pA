using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using InventorySystem;

public class RecipeHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Recipe recipe; // 레시피 참조
    [SerializeField] private TooltipManager tooltipManager;

    private void Awake()
    {
        tooltipManager = FindObjectOfType<TooltipManager>();
        Debug.Assert(tooltipManager != null, "TooltipManager not found in the scene", this);

        Button button = GetComponent<Button>();
        Debug.Assert(button != null, "RecipeHoverHandler requires a Button component", this);
    }

    public void SetRecipe(Recipe newRecipe)
    {
        recipe = newRecipe;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (recipe == null || recipe.resultItem == null)
        {
            Debug.LogWarning("Recipe or resultItem is null in RecipeHoverHandler", this);
            return;
        }
        Vector2 mousePos = eventData.position;
        tooltipManager.ShowTooltip(recipe.resultItem.itemName, recipe.resultItem.itemDescription, mousePos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }
}