using NavMeshPlus.Components;
using System.Collections.Generic;
using Trade;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Chest _chestPrefab;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<LevelInfo> _levelInfos;
    [SerializeField] private List<GameObject> _maps;
    [SerializeField] private ItemsData _itemData;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField] private Ladder _ladderToStore;
    [SerializeField] private Ladder _ladderToArena;
    [SerializeField] private Trader _trader;
    [SerializeField] private EnemyFactory _enemyFactory;

    private List<Enemy> _enemies;
    private List<Item> _items;
    private Chest _chest;
    private Enemy _nearestEnemy;
    private float _minDist;
    private float _dist;
    private int _currentLevelIndex;
    private int _enemyCount;
    private GameObject _map;
    private LevelInfo _levelInfo;
    private bool _win = false;

    public GameObject GetPlayer => _player;
    public ItemsData ItemsData => _itemData;
    public int Level => _currentLevelIndex;
    public LevelInfo LevelInfo => _levelInfo;
    public int EnemyCount
    {
        get => _enemyCount;
        set
        {
            _enemyCount = value;
            if (_enemyCount == 0)
                Win();
        }
    }

    public event System.Action OnLevelCompleted;
    public event System.Action OnLevelLoaded;
    public event System.Action OnGameCompleted;
    public event System.Action OnRestarted;

    private void Awake()
    {
        _ladderToStore.OnPlayerTeleported += () => LoadNextLevelPart1(_win);
        _ladderToArena.OnPlayerTeleported += () => LoadNextLevelPart2(_win);
        MenuController.OnAllRestarted += () => RestartGame();
        RestartPanel.OnRestarted += () => Reset();
        OnLevelCompleted += () =>
        {
            if(_chest)
                _chest.gameObject.SetActive(true);
        };
        _currentLevelIndex = PlayerPrefs.GetInt("_currentLevelIndex");
        _currentLevelIndex = 9;
    }

    private void Start()
    {
        LoadNextLevelPart1(true);
        LoadNextLevelPart2(true);
    }

    public Enemy GetNearestEnemy(Vector3 playerPos)
    {
        if (_enemies.Count == 0)
            return null;
        else if (_enemies.Count == 1)
            return _enemies[0];

        _minDist = float.MaxValue;
        foreach (var enemy in _enemies)
        {
            _dist = (Vector3.SqrMagnitude(enemy.transform.position - playerPos));
            if (!enemy.IsDead && _dist < _minDist)
            {
                _minDist = _dist;
                _nearestEnemy = enemy;
            }
        }
        return _nearestEnemy;
    }

    private void Reset()
    {
        LoadNextLevelPart1(true);
        _win = true;
        OnRestarted?.Invoke();
    }

    private void RestartGame()
    {
        _currentLevelIndex = 0;
        LoadNextLevelPart1(true);
    }

    private void LoadNextLevelPart1(bool win)
    {
        if(win)
        {
            LoadMap();
        }
    }

    private void LoadNextLevelPart2(bool win)
    {
        if (win)
        {
            _win = false; 
            _navMeshSurface.BuildNavMesh(); 
            EnemyCount = _levelInfo.Enemies.Length;
            foreach (var enemyInfo in _levelInfo.Enemies)
            {
                var enemy = _enemyFactory.SpawnEnemy(enemyInfo);
                enemy.OnDeadth += () => EnemyCount--;
                _enemies.Add(enemy);
            }
            foreach (var chestInfo in _levelInfo.LevelChestInfos)
            {
                _chest = Instantiate(_chestPrefab, chestInfo.position, Quaternion.identity);
                _chest.gameObject.SetActive(false);
                _chest.SetItems(chestInfo.itemNames, chestInfo.totalPrise);
            }
        }
    }

    private void LoadMap()
    {
        ClearLevel();
        _levelInfo = _levelInfos[_currentLevelIndex];
        _map = Instantiate(_maps[_currentLevelIndex], transform);

        foreach(var itemInfo in _levelInfo.Items)
        {
            _items.Add(ItemFactoty.Instance.CreateItem(_itemData.GetItem(itemInfo.itemName, 1, itemInfo.itemType), itemInfo.position));
        }
        PlayerPrefs.SetInt("_currentLevelIndex", _currentLevelIndex);
        OnLevelLoaded?.Invoke();
    }

    private void ClearLevel()
    {
        if (_enemies != null)
        {
            foreach (var enemy in _enemies)
                _enemyFactory.DestroyEnemy(enemy);
        }
        _enemies = new List<Enemy>();
        if (_items != null)
        {
            foreach (var item in _items)
            {
                if (item != null)
                    ItemFactoty.Instance.DestroyItem(item);
            }
        }
        _items = new List<Item>();
        if (_chest != null)
        {
            Destroy(_chest.gameObject);
        }
        if (_map != null)
            Destroy(_map.gameObject);
    }

    private void Win()
    {
        _currentLevelIndex++;
        if (_chest != null)
            _chest.gameObject.SetActive(true);
        _win = true;
        OnLevelCompleted?.Invoke();

    }
}
