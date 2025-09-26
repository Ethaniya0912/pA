using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName; // 레시피 이름
    public Sprite previewIcon; // 미리보기 아이콘
    public ItemData resultItem; // 결과 아이템
    public List<ItemRequirement> requirements = new List<ItemRequirement>(); // 필요 재료
    public RecipeCategory category; // 탭 카테고리
}

[System.Serializable]
public class ItemRequirement
{
    public ItemData item; // 필요 아이템
    public int quantity; // 필요 수량
}