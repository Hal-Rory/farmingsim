using Items;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Create New Recipe")]
public class CraftingRecipe: ScriptableObject, IFilterable
{
    [SerializeField] private Item[] Ingredients;
    
    public Item Result;

    [field:SerializeField] public string ID {get; private set;}

    public IEnumerable<Item> GetIngredients()
    {
        for (int i = 0; i < CraftingManager.RecipeIngredientMax; i++)
        {
            if (i >= Ingredients.Length) yield break;
            yield return Ingredients[i];
        }        
    }
}
