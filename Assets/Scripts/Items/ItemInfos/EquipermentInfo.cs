using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class EquipermentInfo : ItemInfo
{
    [SerializeField] protected EquipmentType _equipmentType;
    [SerializeField] protected List<EquipmentBonus> _bonus;
    [SerializeField] protected WeaponType _weaponType;

    private string _info;

    public EquipmentType EquipmentType => _equipmentType;
    public List<EquipmentBonus> Bonus => _bonus;
    public WeaponType WeaponType => _weaponType;

    public EquipermentInfo() { }

    public EquipermentInfo(Sprite sprite, int prise, string name, ItemType type, EquipmentType equipmentType, List<EquipmentBonus> equipmentBonus)
    {
        _sprite = sprite;
        _prise = prise;
        _name = name;
        _type = type;
        _equipmentType = equipmentType;
        _bonus = equipmentBonus;
    }

    public EquipermentInfo (EquipermentInfo other)
    {
        _sprite = other._sprite;
        _prise = other._prise;
        _name = other._name;
        _type = other._type;
        _description = other._description;

        _bonus = other._bonus;
        _weaponType = other._weaponType;
        _equipmentType = other._equipmentType;
    }

    public override string GetInfo()
    {
        _info = "";
        if (_info.Length > 0)
            return _info;
        else
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var bonus in _bonus)
            {
                stringBuilder.AppendLine($"{bonus.bonusType} {bonus.value}");
            }
            stringBuilder.AppendLine(base.GetInfo());
            _info = stringBuilder.ToString();
            return _info;
        }
    }
}

[System.Serializable]
public struct EquipmentBonus
{
    public BonusType bonusType;
    public int value;
}

public enum BonusType
{
    HP,
    Damage,
    Armor
}

public enum WeaponType
{
    None,
    Axe,
    Sword
}
