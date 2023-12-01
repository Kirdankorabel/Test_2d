using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemsData", order = 1)]
public class ItemsData : ScriptableObject
{
    [SerializeField] private List<ItemInfo> _items;
    [SerializeField] private List<EquipermentInfo> _equiperments;
    [SerializeField] private List<ConsumableInfo> _consumables;
    [SerializeField] private List<ItemImage> _images;
    [SerializeField] private string _moneyName = "Money";
    [SerializeField] private string _randomName = "Random";

    public ItemInfo GetItem(string itemName, int value = 0, ItemType type = 0)
    {
        if(itemName == "Random")
            return GetRandomItem();
        switch (type)
        {
            case ItemType.Equiperment:
                return new EquipermentInfo(_equiperments.Find(item => item.Name == itemName));
            case ItemType.Consumable:
                return new ConsumableInfo(_consumables.Find(item => item.Name == itemName));
            case ItemType.Valuable:
                return new ItemInfo(_items.Find(item => item.Name == itemName));
            default:
                ItemInfo item = null;
                ItemInfo itemInfo;
                itemInfo = _consumables.Find(i => i.Name == itemName);
                if (itemInfo != null)
                {
                    return new ConsumableInfo((ConsumableInfo)itemInfo);
                }
                if (itemInfo == null)
                {
                    itemInfo = _equiperments.Find(i => i.Name == itemName);
                    if (itemInfo != null)
                    {
                        return new EquipermentInfo((EquipermentInfo)itemInfo);
                    }
                }
                if (itemInfo == null)
                {
                    itemInfo = _items.Find(i => i.Name == itemName);
                    if (itemInfo != null)
                    {
                        return new ItemInfo((EquipermentInfo)itemInfo);
                    }
                }
                return item;
        }
    }

    public Sprite GetSprite(string name)
    {
        return _images.Find(i => i.name == name).sprite;
    }

    public ItemInfo GetMoney(int count)
    {
        var item = new ConsumableInfo(_consumables.Find(item => item.Name == _moneyName));
        item.SetValue(count);
        return item;
    }

    private ItemInfo GetRandomItem()
    {
        var item = GetItem(_images[Random.Range(0, _images.Count)].name);
        while (item.Name == _moneyName)
        {
            item = GetRandomItem();
        }
        return item;
    }

    public List<ItemInfo> GetItemsForPrice(int price)
    {
        var items = new List<ItemInfo>();
        ItemInfo item;
        while (price > 0)
        {
            item = GetItem(_images[Random.Range(0, _images.Count)].name);
            if(item.Name != _moneyName)
            {
                items.Add(item);
                price -= item.Price;
            }
        }

        return items;
    }
}

[System.Serializable]
public struct ItemImage
{
    public Sprite sprite;
    public string name;
}
