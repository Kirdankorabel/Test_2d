using UnityEngine;

[System.Serializable]
public class ConsumableInfo : ItemInfo
{
    [SerializeField] protected string _abilityName;
    [SerializeField] protected int _value;

    public int Value => _value;
    public string AbilityName => _abilityName;

    public ConsumableInfo() { }

    public ConsumableInfo(ConsumableInfo other)
    {
        _sprite = other._sprite;
        _prise = other._prise;
        _name = other._name;
        _type = other._type;
        _description = other._description;
        _abilityName = other._abilityName;
        _value = other._value;
    }

    public ConsumableInfo SetValue(int value)
    {
        _value = value;
        return this;
    }
    public override string GetInfo()
    {
        return $"{_name} \nPrice: {_prise * _value} \n{_description}";
    }
}
