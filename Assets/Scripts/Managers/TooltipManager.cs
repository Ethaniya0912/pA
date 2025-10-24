using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipTitle; // TooltipText
    [SerializeField] private TextMeshProUGUI tooltipText; // TooltipText
    [SerializeField] private CanvasGroup tooltipGroup; // ���̵� ȿ��
    [SerializeField] private float fadeSpeed = 0.2f; // ���̵� �ӵ�

    private RectTransform tooltipRect; // ��ġ ������

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        tooltipGroup.alpha = 0f; // �ʱ� ����
        //tooltipGroup.gameObject.SetActive(false);
        if (tooltipText == null) Debug.LogError("TooltipText not assigned", this);
    }

    public void ShowTooltip(string name, string description, Vector2 mousePos)
    {
        if (tooltipText == null) return;
        tooltipTitle.text = name;
        tooltipText.text = description;

        // ��ġ ���� (���콺 ������)
        tooltipRect.position = mousePos + new Vector2(20f, -20f); // ������

        tooltipGroup.gameObject.SetActive(true);
        tooltipGroup.alpha = 1f; // ��� ǥ��
        //Debug.Log("Tooltip shown: " + name);
    }

    public void HideTooltip()
    {
        tooltipGroup.alpha = 0f; // ���̵� �ƿ� (Lerp�� �ε巴�� ����)
        //tooltipGroup.gameObject.SetActive(false); //SetActive(false) �ϸ� RecipeHoverHandler ���۽� �ν��Ͻ��� ��ã�Ƽ� �ּ�ó����
        //Debug.Log("Tooltip hidden");
    }
}