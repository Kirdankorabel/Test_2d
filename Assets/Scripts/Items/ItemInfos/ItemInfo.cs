using System;
using UnityEngine;

[Serializable]
public class ItemInfo
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected int _prise;
    [SerializeField] protected string _name;
    [SerializeField] protected string _description;
    [SerializeField] protected ItemType _type;

    public string Name => _name;
    public int Price => _prise;
    public ItemType Type => _type;
    public string Description => _description;

    public ItemInfo () { }

    public ItemInfo(Sprite sprite, int prise, string name, ItemType type)
    {
        _sprite = sprite;
        _prise = prise;
        _name = name;
        _type = type;
    }

    public ItemInfo(ItemInfo other)
    {
        _sprite = other._sprite;
        _prise = other._prise;
        _name = other._name;
        _type = other._type;
        _description = other._description;
    }

    public virtual string GetInfo()
    {
        return $"{_name} \nPrice: {_prise} \n{_description}";
    }
}
