using Codice.Client.Common;
using Farm.Field;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUIDock : MonoBehaviour, IUIDock
{
    [SerializeField] private Field Target;
    public virtual void Docked(DockableUI dockable)
    {
        if(dockable.TryGetComponent(out IFilterable filter))
        {
            if(GameManager.Instance.AllCrops.TryGetValue(filter.ID, out CropData toPlant))
            {
                print(toPlant.name);
                Target.PlantNewCrop(toPlant);
            }
        }
    }
}
