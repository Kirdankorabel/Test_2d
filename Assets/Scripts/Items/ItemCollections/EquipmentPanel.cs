using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : ItemCollection
{
    [SerializeField] private List<EquipmentType> _equipments;

    public override bool TryToAddItem(ItemInfo item, int cell = -1)
    {
        return base.TryToAddItem(item);
    }

    protected override int GetFreeCell(ItemInfo itemInfo)
    {
        if (itemInfo.Type == ItemType.Equiperment)
        {
            for (var i = 0; i < _equipments.Count; i++)
            {
                if (_equipments[i] == ((EquipermentInfo)itemInfo).EquipmentType && _cells[i].IsFree)
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
