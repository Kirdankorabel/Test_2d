using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelInfo", order = 1)]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private LevelEnemyInfo[] _enemies;
    [SerializeField] private LevelItemInfo[] _items;
    [SerializeField] private LevelChestInfo[] _chestInfos;
    [SerializeField] private string _name;
    [SerializeField] private Rect _size;
    [SerializeField] private int _traderPrice;

    public string Name => _name;
    public LevelItemInfo[] Items => _items;
    public LevelEnemyInfo[] Enemies => _enemies;
    public LevelChestInfo[] LevelChestInfos => _chestInfos;
    public Rect Size => _size;
    public int TraderPrice => _traderPrice;
}

[Serializable]
public struct LevelEnemyInfo
{
    public Vector3 position;
    public EnemyType enemyType;
    public Characteristics characteristics;
}

[Serializable]
public struct LevelItemInfo
{
    public Vector3 position;
    public string itemName;
    public ItemType itemType;
}

[Serializable]
public struct LevelChestInfo
{
    public Vector3 position;
    public List<string> itemNames;
    public int totalPrise;
}
