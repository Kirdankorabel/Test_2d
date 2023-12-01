using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

public class FastItems : ItemCollection
{
    [SerializeField] private ItemCollection _inventory;
    [SerializeField] private HealthController _healthController;
    [SerializeField] private TMP_Text _text;

    public event System.Action<int> OnItemUsed;

    private void Awake()
    {
        _inventory.OnItemAdded += (value) => TryToAddItem(value);
        _inventory.OnItemRemoved += (value) => TryToRemoveItem(value);
        _healthController.OnHealthChanged.AddListener((value) => _text.text = $"{_healthController.Health} / {Player.Instance.Characteristics.HP}");
    }

    public override bool TryToAddItem(ItemInfo item, int cell = -1)
    {
        if (Items.Contains(item))
            return false;
        base.TryToAddItem(item, cell);
        return false;
    }

    public override bool TryToRemoveItem(ItemInfo item)
    {
        base.TryToRemoveItem(item);
        return false;
    }
    public override bool TryToAddItem(ItemInfo item, bool value)
    {
        return false;
    }

    protected override void UseItem(int cellNubder)
    {
        if (_items[cellNubder] != null && _items[cellNubder].Type == ItemType.Consumable)
        {
            _consumableInfo = (ConsumableInfo)_items[cellNubder];

            MethodInfo staticMethod = staticClassType.GetMethod(_consumableInfo.AbilityName);
            staticMethod.Invoke(null, new object[] { _consumableInfo.Value });

            _inventory.TryToRemoveItem(_items[cellNubder]);
            TryToRemoveItem(_items[cellNubder]); 
        }
    }
}
