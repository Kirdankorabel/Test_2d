using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Characteristics
{
    public int HP;
    public int damage;
    public int armor;
    public int minDamage = 2;
    public int minHP = 20;
    public float cooldown = 1f;
    public float timeToDamage = 0.4f;
    public float attackDist = 1.5f;
    public float speed = 5;
    public WeaponType weaponType = WeaponType.None;

    public Characteristics() { }
    public Characteristics(int hP, int damage, int armor, float cooldown, float timeToDamage, float attackDist, WeaponType weaponType)
    {
        HP = hP;
        this.damage = damage;
        this.armor = armor;
        this.cooldown = cooldown;
        this.timeToDamage = timeToDamage;
        this.attackDist = attackDist;
        this.weaponType = weaponType;

    }

    public Characteristics(ItemInfo[] infos)
    {
        HP = minHP;
        damage = 0; 
        armor = 0;
        EquipermentInfo item;
        weaponType = WeaponType.None;
        foreach(var info in infos)
        {
            item = info as EquipermentInfo;
            if (info != null && info.Type == ItemType.Equiperment)
            {
                item = info as EquipermentInfo;
                foreach (var bonus in item.Bonus)
                {
                    switch (bonus.bonusType)
                    {
                        case BonusType.Armor:
                            armor += bonus.value;
                            break;
                        case BonusType.HP:
                            HP += bonus.value;
                            break;
                        case BonusType.Damage:
                            damage += bonus.value;
                            break;

                    }
                }
                if (item.EquipmentType == EquipmentType.Weapon)
                    weaponType = item.WeaponType;
            }
        }

        damage = damage > minDamage ? damage : 2;
    }
}
