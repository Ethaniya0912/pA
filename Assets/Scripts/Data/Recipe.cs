using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName; // ������ �̸�
    public Sprite previewIcon; // �̸����� ������
    public ItemData resultItem; // ��� ������
    public List<ItemRequirement> requirements = new List<ItemRequirement>(); // �ʿ� ���
    public RecipeCategory category; // �� ī�װ�
}

[System.Serializable]
public class ItemRequirement
{
    public ItemData item; // �ʿ� ������
    public int quantity; // �ʿ� ����
}