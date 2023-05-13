using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    [SerializeField] private ProfileInfoPanel ProfilePanel;

    [SerializeField] private Dictionary<CraftingRecipe, KeyValuePair<InfoHeader, ListView>> CategoryViews = new Dictionary<CraftingRecipe, KeyValuePair<InfoHeader, ListView>>();
    [SerializeField] private RectTransform Container;

    [SerializeField] private GameObject ToggleCardPrefab;
    [SerializeField] private GameObject CategoryPrefab;    

    [SerializeField] private Card[] IngredientPanel;
    
    [SerializeField] private Button Confirmation;
    private CraftingRecipe CurrentRecipe;    
    private ToggleCard CurrentToggle;    
    
    [SerializeField] private Card TooltipCard;
    [SerializeField] private ToggleGroup Group;
    private CraftingManager CraftingManager => GameManager.Instance.CraftingManager;
    void Start()
    {        
        Assert.IsTrue(IngredientPanel.Length==CraftingManager.RecipeIngredientMax, "Wrong amount of Cards in Ingredient Panel");
        CloseLastDisplay();
        SetupRecipes();
        GameManager.Instance.OnItemUpdated += DoItemsUpdated;
        Confirmation.onClick.AddListener(CreateRecipe);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnItemUpdated -= DoItemsUpdated;
        Confirmation.onClick.RemoveAllListeners();
    }
    /// <summary>
    /// Go through RECIPES and turn off cards with missing ingredients, then check if panel is open and do the same
    /// </summary>
    /// <param name="item"></param>
    private void DoItemsUpdated(Item item)
    {
        foreach (var recipe in CategoryViews.Keys)
        {
            if (CategoryViews[recipe].Value.TryGetItem(recipe.ID, out ToggleCard recipeCard))
            {
                bool available = SetRecipeStatus(recipe, recipeCard.gameObject.GetComponent<Colorable>());
                if (recipe == CurrentRecipe)
                {
                    SetIngredientCards(item);
                    Confirmation.interactable = available;
                }
            }
        }
    }

    private void OnEnable()
    {
        Confirmation.interactable = CheckInventory(CurrentRecipe);
        CloseLastDisplay();
    }

    public void CreateRecipe()
    {
        CurrentRecipe.Create();        
    }

    private void CloseLastDisplay()
    {
        for (int i = 0; i < IngredientPanel.Length; i++)
        {
            IngredientPanel[i].gameObject.SetActive(false);
            IngredientPanel[i].SetEmpty();
        }
        ProfilePanel.SetEmpty();
        ProfilePanel.SetHeader("none", "Select an item to inspect.");
        Confirmation.interactable = false;
        CurrentRecipe = null;
        if(CurrentToggle!= null) CurrentToggle.SetIsOnWithoutNotify(false);
        CurrentToggle = null;
    }
    private void SetRecipe(CraftingRecipe recipe)
    {
        CurrentRecipe = recipe;
        ProfilePanel.SetPanel(CurrentRecipe.ID, recipe.Result.Data.Name, string.Empty, recipe.Result.Data.Display);        
        SetIngredientCards();
        Confirmation.interactable = CheckInventory(CurrentRecipe);
    }
    private void SetIngredientCards(Item item = null)
    {
        Item[] list = CurrentRecipe.GetIngredients().ToArray();
        for (int i = 0; i < IngredientPanel.Length; i++)
        {
            Item recipeItem = list[Mathf.Clamp(i, 0, list.Length-1)];
            Colorable colorable = IngredientPanel[i].GetComponent<Colorable>();
            if (item != null)
            {
                if (IngredientPanel[i].ID == item.Data.ID)
                {
                    if (CheckInventory(recipeItem)) colorable.SetPrimary();
                    else colorable.SetSecondary();
                    return;
                }
            }
            else
            {
                if (i < list.Length)
                {
                    IngredientPanel[i].gameObject.SetActive(true);
                    IngredientPanel[i].Set(recipeItem.Data.ID, recipeItem.Amount.ToString(), recipeItem.Data.Display);
                    if (CheckInventory(recipeItem)) colorable.SetPrimary();
                    else colorable.SetSecondary();
                }
                else
                {
                    IngredientPanel[i].gameObject.SetActive(false);
                    IngredientPanel[i].SetEmpty();
                }
            }
        }
    }
    public void OnIngredientHoverEnter(Card card)
    {
        if (!CurrentRecipe.TryGetIngredient(card.ID, out Item item) || (card.GetComponent<Hoverable>() is not Hoverable hover)) return;
        TooltipCard.Set(item.Data.ID, item.Data.Name, null);
        TooltipCard.gameObject.SetActive(true);
        TooltipManager.Instance.SetCard(card.GetComponent<Hoverable>(), TooltipCard.gameObject, transform);
    }
    public void OnIngredientHoverExit()
    {
        TooltipCard.gameObject.SetActive(false);
        TooltipManager.Instance.RemoveLast(transform);
    }
    private KeyValuePair<InfoHeader, ListView> CreateCategory(CraftingRecipe id, string label)
    {
        GameObject category = Instantiate(CategoryPrefab, Container);
        InfoHeader header = category.GetComponent<InfoHeader>();
        header.SetHeader(id.Category, label);
        ListView listview = category.GetComponent<ListView>();
        KeyValuePair<InfoHeader, ListView> categoryPair = new KeyValuePair<InfoHeader, ListView>(header, listview);        
        LayoutRebuilder.ForceRebuildLayoutImmediate(Container);
        category.GetComponent<Collapsible>().OnExpanded += (expanded) => LayoutRebuilder.ForceRebuildLayoutImmediate(Container);
        return categoryPair;
    }
    
    private bool TryGetCategory(string category, out KeyValuePair<InfoHeader, ListView> pair)
    {
        pair = default;
        foreach (var item in CategoryViews)
        {
            if(item.Key.Category == category)
            {
                pair = item.Value;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Create recipe categories and cards, set card inactive if ingredients are missing
    /// </summary>
    public void SetupRecipes()
    {
        IEnumerable<CraftingRecipe> recipes = CraftingManager.GetAllRecipes();
        foreach (var recipe in recipes)
        {
            if(!TryGetCategory(recipe.Category, out KeyValuePair<InfoHeader, ListView> pair))
            {
                pair = CreateCategory(recipe, recipe.Category);
            }
            CategoryViews.Add(recipe, pair);
            ToggleCard recipeCard = pair.Value.AddCard<ToggleCard>(recipe.ID, ToggleCardPrefab, true);
            recipeCard.SetGroup(Group);

            void SelectRecipe(bool selected)
            {
                if (selected)
                {
                    SetRecipe(recipe);
                    CurrentToggle = recipeCard;
                }
                else
                {
                    if (CurrentRecipe != null && CurrentRecipe.ID == recipe.ID || CurrentRecipe == null)
                    {
                        CloseLastDisplay();
                    }
                }
                Confirmation.interactable = CheckInventory(CurrentRecipe);
            }
            
            recipeCard.Set(recipe.ID, recipe.Result.Data.Name, recipe.Result.Data.Display, SelectRecipe);
            
            SetRecipeStatus(recipe, recipeCard.gameObject.GetComponent<Colorable>());

            if (!recipeCard.TryGetComponent(out Hoverable hoverable))
            {
                hoverable = recipeCard.gameObject.AddComponent<Hoverable>();
            }
            hoverable.PointerEnter.RemoveAllListeners();
            hoverable.PointerExit.RemoveAllListeners();
            hoverable.PointerEnter.AddListener((hoverable) =>
            {
                TooltipCard.Set(recipeCard.ID, recipe.Result.Data.Name, null);
                TooltipCard.gameObject.SetActive(true);
                TooltipManager.Instance.SetCard(hoverable, TooltipCard.gameObject, transform);
            });
            hoverable.PointerExit.AddListener((hoverable) =>
            {
                TooltipCard.gameObject.SetActive(false);
                TooltipManager.Instance.RemoveLast(transform);
            });

        }
    }

    private bool SetRecipeStatus(CraftingRecipe recipe, Colorable colorable)
    {
        if (CheckInventory(recipe))
        {
            colorable.SetPrimary();
            return true;
        }
        else
        {
            colorable.SetSecondary();
            return false;
        }
    }


    private bool CheckInventory(Item recipeItem)
    {
        return GameManager.Instance.ContainsItem(recipeItem.Data, recipeItem.Amount);
    }
    private bool CheckInventory(CraftingRecipe recipe)
    {
        if(recipe == null) return false;
        foreach (var item in recipe.GetIngredients())
        {
            if(!CheckInventory(item)) return false;
        }
        return true;
    }
}
