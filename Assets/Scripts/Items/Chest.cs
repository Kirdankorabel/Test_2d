using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractingObject
{
    [SerializeField] private List<string> _itemNames;
    private List<ItemInfo> _items;

    public List<ItemInfo> Items => _items;

    public static event System.Action<Chest> OnChestOpened;

    private void Awake()
    {
        _items = new List<ItemInfo>();
        foreach (var itemName in _itemNames)
            _items.Add(GameController.Instance.ItemsData.GetItem(itemName));
    }

    public override void Use(GameObject user)
    {
        OnChestOpened?.Invoke(this);
    }

    public void SetItems(List<string> items, int totalPtice)
    {
        _itemNames = items;
        foreach (var itemName in _itemNames)
            _items.Add(GameController.Instance.ItemsData.GetItem(itemName));

        foreach (var item in _items)
        {
            totalPtice -= item.Price;
        }
        if (totalPtice > 0)
        {
            _items.Add(GameController.Instance.ItemsData.GetMoney(totalPtice));
        }
    }

    public void SetItems(List<ItemInfo> items)
    {
        _items = items;
    }
}
