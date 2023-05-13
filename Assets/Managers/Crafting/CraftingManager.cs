using System.Collections.Generic;
using UnityEngine;

public class CraftingManager {
    public static int RecipeIngredientMax => 6;
    private List<CraftingRecipe> AllRecipes = new List<CraftingRecipe>();
	public CraftingManager()
	{
        List<CraftingRecipe> recipes = new List<CraftingRecipe>(Resources.LoadAll<CraftingRecipe>("Recipes"));
        foreach (CraftingRecipe recipe in recipes)
        {
            AllRecipes.Add(recipe);
        }
    }
    public IEnumerable<CraftingRecipe> GetAllRecipes()
    {
        return AllRecipes;
    }
}
