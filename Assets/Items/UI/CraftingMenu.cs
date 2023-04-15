using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    [SerializeField] private ListView Display;
    [SerializeField] private GameObject ToggleCardPrefab;
    
    [SerializeField] private ProfileInfoPanel ProfilePanel;
    [SerializeField] private Card[] IngredientPanel;
    [SerializeField] private Button Confirmation;
    private CraftingRecipe CurrentRecipe;    
    private ToggleCard CurrentToggle;    
    
    [SerializeField] private ToggleGroup Group;
    private CraftingManager CraftingManager => GameManager.Instance.CraftingManager;
    void Start()
    {        
        Assert.IsTrue(IngredientPanel.Length==CraftingManager.RecipeIngredientMax, "Wrong amount of Cards in Ingredient Panel");
        CloseLastDisplay();
        SetupRecipes();
    }
    private void OnEnable()
    {
        Confirmation.interactable = CheckInventory(CurrentRecipe);
        CloseLastDisplay();
    }

    public void CreateRecipe()
    {
        foreach (var item in CurrentRecipe.GetIngredients())
        {
            GameManager.Instance.RemoveItem(item.Data, item.Amount);
        }
        GameManager.Instance.AddItem(CurrentRecipe.Result.Data, CurrentRecipe.Result.Amount);
        Confirmation.interactable = CheckInventory(CurrentRecipe);
    }

    private void CloseLastDisplay()
    {
        for (int i = 0; i < IngredientPanel.Length; i++)
        {
            IngredientPanel[i].gameObject.SetActive(false);
            IngredientPanel[i].SetEmpty();
        }
        ProfilePanel.SetEmpty();
        Confirmation.interactable = false;
        CurrentRecipe = null;
        if(CurrentToggle!= null) CurrentToggle.Selectable.SetIsOnWithoutNotify(false);
        CurrentToggle = null;
    }
    private void SetRecipe(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;
        ProfilePanel.SetPanel(CurrentRecipe.ID, recipe.Result.Data.Name, recipe.Result.Data.Description, recipe.Result.Data.Display);
        Item[] list = recipe.GetIngredients().ToArray();
        for (int i = 0; i < IngredientPanel.Length; i++)
        {
            if (i < list.Length)
            {
                IngredientPanel[i].gameObject.SetActive(true);
                IngredientPanel[i].Set(i.ToString(), list[i].Amount.ToString(), list[i].Data.Display);
            }
            else
            {
                IngredientPanel[i].gameObject.SetActive(false);
                IngredientPanel[i].SetEmpty();
            }
        }
    }

    public void SetupRecipes()
    {
        IEnumerable<CraftingRecipe> recipes = CraftingManager.GetAllRecipes();
        foreach (var recipe in recipes)
        {
            ToggleCard card = Display.AddCard<ToggleCard>(recipe.ID, ToggleCardPrefab);            
            card.Selectable.group = Group;
            void SelectRecipe(bool selected)
            {
                if (selected)
                {
                    SetRecipe(recipe);
                    CurrentToggle = card;
                }
                else
                {
                    if(CurrentRecipe != null && CurrentRecipe.ID == recipe.ID || CurrentRecipe == null)
                    {
                        CloseLastDisplay();
                    }
                }
                Confirmation.interactable = CheckInventory(CurrentRecipe);
            }
            card.Set(recipe.ID, recipe.Result.Data.Name, recipe.Result.Data.Display, SelectRecipe);
        }
    }

    private bool CheckInventory(CraftingRecipe recipe)
    {
        if(recipe == null) return false;        
        foreach (var item in recipe.GetIngredients())
        {
            if (!GameManager.Instance.ContainsItem(item.Data, item.Amount)) return false;            
        }
        return true;
    }
}
