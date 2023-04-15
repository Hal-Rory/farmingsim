using Farm.Field;
using Items;
using UnityEngine;

public class FieldUIDock : MonoBehaviour, IUIDock
{
    [SerializeField] private SeedPlot Target;
    public virtual void Docked(DockableUI dockable)
    {
        if(dockable.TryGetComponent(out IFilterable filter))
        {
            if(GameManager.Instance.AllCrops.TryGetValue(filter.ID, out SeedData toPlant))
            {
                print(toPlant.name);
                Target.PlantNewCrop(toPlant);
            }
        }
    }
}
