using Farm.Field;
using UnityEngine;
using UnityEngine.UI;

public class FieldStatusUI : MonoBehaviour
{
    public Card DisplayCard;
    public Slider DisplaySlider;
    private Field Target;
    private void Start()
    {
        DisplayCard.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (GameManager.Instance.TryGetCurrentHovered(SELECTABLE_TYPE.field, out GameObject selectable))
        {
            Target = selectable.GetComponent<Field>();
            if (!DisplayCard.gameObject.activeSelf)
            {                
                DisplayCard.gameObject.SetActive(true);
            }
            if(Target.HasCrop()) DisplayCard.SetIcon(Target.CropStats.Display);
        } else if(Target != null)
        {
            Target = null;
            DisplayCard.gameObject.SetActive(false);
        }  
        
        if(Target!= null)
        {
            if (DisplayCard.Icon.IsActive() != Target.HasCrop())
            {
                DisplayCard.SetIcon(Target.HasCrop());
            }
            DisplaySlider.value = Target.WaterLevel;
            DisplayCard.SetLabel(Target.HasCrop() ? $"{Target.GrowthLevel*100:0.##}%" : "No crop");
        }
    }
}
