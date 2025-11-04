using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipTitle; // TooltipText
    [SerializeField] private TextMeshProUGUI tooltipText; // TooltipText
    [SerializeField] private CanvasGroup tooltipGroup; // 페이드 효과
    //[SerializeField] private float fadeSpeed = 0.2f; // 페이드 속도

    private RectTransform tooltipRect; // 위치 조정용

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        tooltipGroup.alpha = 0f; // 초기 숨김
        //tooltipGroup.gameObject.SetActive(false);
        if (tooltipText == null) Debug.LogError("TooltipText not assigned", this);
    }

    public void ShowTooltip(string name, string description, Vector2 mousePos)
    {
        if (tooltipText == null) return;
        tooltipTitle.text = name;
        tooltipText.text = description;

        // 위치 조정 (마우스 오른쪽)
        tooltipRect.position = mousePos + new Vector2(20f, -20f); // 오프셋

        tooltipGroup.gameObject.SetActive(true);
        tooltipGroup.alpha = 1f; // 즉시 표시
        //Debug.Log("Tooltip shown: " + name);
    }

    public void HideTooltip()
    {
        tooltipGroup.alpha = 0f; // 페이드 아웃 (Lerp로 부드럽게 가능)
        //tooltipGroup.gameObject.SetActive(false); //SetActive(false) 하면 RecipeHoverHandler 동작시 인스턴스를 못찾아서 주석처리함
        //Debug.Log("Tooltip hidden");
    }
}