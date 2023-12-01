using TMPro;
using Trade;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Singleton<PlayerInventory>
{
    [SerializeField] private TriggerController _triggerController;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Image _selectedItemIcon;
    [SerializeField] private ItemCollection _itemsCollection;
    [SerializeField] private EquipmentPanel _equipmentPanel;
    [SerializeField] private TraderInventory _traderInventory;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private int _startMoney;

    private int _money = 100;

    public ItemInfo[] Items => _itemsCollection.Items;
    public ItemInfo[] Equipment => _equipmentPanel.Items;
    public int Money
    {
        get => _money;
        private set
        {
            _money = value;
            _moneyText.text = _money.ToString();
        }
    }

    public event System.Action<int> OnMoneyChanged;
    public event System.Action OnItemAdded;

    private void Awake()
    {
        _traderInventory.OnMonetChacged += (value) => Money += value;
        _triggerController.OnItemPickUp += (value) => _itemsCollection.TryToAddItem(value);
        AbilityUser.OnMoneyAdded += (value) => Money += value;
        GameController.Instance.OnRestarted += () => Reset();
        GameController.Instance.OnLevelCompleted += () => SaveItems();
        _equipmentPanel.OnItemAdded += (value) => OnItemAdded?.Invoke();
        _equipmentPanel.OnItemRemoved += (value) => OnItemAdded?.Invoke();
    }

    void Start()
    {
        if (GameController.Instance.Level > 0)
            LoadItems();
        _moneyText.text = _money.ToString();
        _inventoryPanel.SetActive(false); 
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (!_inventoryPanel.active)
            {
                _equipmentPanel.gameObject.SetActive(true);
                _inventoryPanel.SetActive(true);
            }
            else
            {
                _inventoryPanel.SetActive(false);
            }
        }
    }

    private void Reset()
    {
        if (GameController.Instance.Level > 0)
            LoadItems();
        Debug.LogError(GameController.Instance.Level);
    }

    public void Show(bool value)
    {
        _inventoryPanel.gameObject.SetActive(value);
        _equipmentPanel.gameObject.SetActive(!value);
    }

    private void LoadItems()
    {
        Money = PlayerPrefs.GetInt("PlayerMoney");
        var collection = JsonUtility.FromJson<ItemsArray>(SaveManager.LoadData("Items.json"));
        if(collection != null)
        {
            for (var i = 0; i < collection.items.Length; i++)
            {
                if (!string.IsNullOrEmpty(collection.items[i]))
                    _itemsCollection.TryToAddItem(GameController.Instance.ItemsData.GetItem(collection.items[i], collection.values[i]), i);
            }
        }
        collection = JsonUtility.FromJson<ItemsArray>(SaveManager.LoadData("Equipment.json")); 
        if (collection != null)
        {
            for (var i = 0; i < collection.items.Length; i++)
            {
                if (!string.IsNullOrEmpty(collection.items[i]))
                    _equipmentPanel.TryToAddItem(GameController.Instance.ItemsData.GetItem(collection.items[i]), i);
            }
        }
    }

    private void SaveItems(bool isNew = false)
    {
        PlayerPrefs.SetInt("PlayerMoney", _money);
        ItemsArray items = new ItemsArray();
        var array = new string[_itemsCollection.Items.Length];
        var values = new int[_itemsCollection.Items.Length];
        if (!isNew)
            for (var i = 0; i < _itemsCollection.Items.Length; i++)
            {
                if (_itemsCollection.Items[i] != null)
                {
                    array[i] = _itemsCollection.Items[i].Name;
                    if (_itemsCollection.Items[i].Type == ItemType.Consumable)
                        values[i] = ((ConsumableInfo)_itemsCollection.Items[i]).Value;
                    else
                        values[i] = 0;
                }
            }
        items.items = array;
        items.values = values;
        SaveManager.SaveData(JsonUtility.ToJson(items), "Items.json");
        array = new string[_equipmentPanel.Items.Length];
        values = new int[_equipmentPanel.Items.Length];
        if (!isNew)
            for (var i = 0; i < _equipmentPanel.Items.Length; i++)
            {
                if (_equipmentPanel.Items[i] != null)
                    array[i] = _equipmentPanel.Items[i].Name;
            }
        items.items = array;
        items.items = array;
        SaveManager.SaveData(JsonUtility.ToJson(items), "Equipment.json");
    }

    private void OnApplicationQuit()
    {
        if (GameController.Instance.Level > 0)
            SaveItems();
    }
}
[System.Serializable]
public class ItemsArray
{
    [SerializeField] public string[] items;
    [SerializeField] public int[] values;
    public ItemsArray() { }
}