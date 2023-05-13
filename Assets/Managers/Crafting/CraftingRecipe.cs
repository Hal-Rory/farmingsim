using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Create New Recipe")]
public class CraftingRecipe: ScriptableObject, IFilterable
{
    [Serializable]
    internal class RecipeItem
    {
        public Item Ingredient;
        public bool RemoveOnCreation = true;
    }

    [SerializeField] private RecipeItem[] Ingredients;
    
    public Item Result;
    public string Category;
    [field:SerializeField] public string ID {get; private set;}

    public IEnumerable<Item> GetIngredients()
    {
        for (int i = 0; i < CraftingManager.RecipeIngredientMax; i++)
        {
            if (i >= Ingredients.Length) yield break;
            yield return Ingredients[i].Ingredient;
        }        
    }
    public bool TryGetIngredient(string ID, out Item item)
    {
        item = null;
        for (int i = 0; i < CraftingManager.RecipeIngredientMax; i++)
        {
            if (Ingredients[i].Ingredient.Data.ID == ID)
            {
                item = Ingredients[i].Ingredient;
                return true;
            }
        }
        return false;
    }

    public void Create()
    {
        GameManager.Instance.AddItem(Result.Data, Result.Amount);
        for (int i = 0; i < CraftingManager.RecipeIngredientMax; i++)
        {
            if (i >= Ingredients.Length) break;
            if(Ingredients[i].RemoveOnCreation) GameManager.Instance.RemoveItem(Ingredients[i].Ingredient.Data, Ingredients[i].Ingredient.Amount);
        }
    }
}
