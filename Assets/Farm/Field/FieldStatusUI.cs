using Farm.Field;
using UnityEngine;
using UnityEngine.UI;

public class FieldStatusUI : UIPage
{
    public Card DisplayCard;
    public Slider DisplaySlider;
    private SeedPlot Target;
    private bool DisplaySet;
    protected override void Start()
    {
        base.Start();
        DisplayCard.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (GameManager.Instance.Selection.TryGetCurrentHovered(out ISelectable selectable) && selectable is SeedPlot field)
        {
            Target= field;
            if (!DisplayCard.gameObject.activeSelf)
            {
                DisplaySet = false;
                DisplayCard.gameObject.SetActive(true);
            }
            if(Target.HasCrop()) DisplayCard.SetIcon(Target.CropStats.Display);
        } else if(Target != null)
        {
            Target = null;
            DisplaySet = false;
            DisplayCard.gameObject.SetActive(false);
        }  
        
        if(Target != null)
        {
            if (!DisplaySet)
            {
                DisplaySet = true;
                DisplayCard.SetIcon(Target.CropStats?.Display);
            }
            DisplaySlider.value = Target.WaterLevel;
            float status = Target.GrowthLevel;
            if (status < 0)
            {
                DisplayCard.SetLabel("Crop has died.");
            }
            else if (status >= 0 && status < 1 && Target.HasCrop())
            {
                DisplayCard.SetLabel($"{status * 100:0}%");
            }
            else if (status == 0)
            {
                DisplayCard.SetLabel($"No crop here.");
            }
            else
            {
                DisplayCard.SetLabel("Ready to Harvest!");
            }
        }
    }
}
