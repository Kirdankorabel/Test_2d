using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCollection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected ItemInfo[] _items;
    [SerializeField] protected Cell[] _cells;

    protected ConsumableInfo _consumableInfo;
    protected Type staticClassType = typeof(AbilityUser);
    protected int _selectedCellIndx = 0;
    private int _itemsCount;
    private Item _selectedItem;
    private bool _isSelected;

    [SerializeField]
    public ItemInfo[] Items
    {
        get
        {
            if (_items == null || _items.Length == 0)
            {
                _items = new ItemInfo[_cells.Length];
            }
            return _items;
        }
        protected set
        {
            _items = new ItemInfo[value.Length];
            foreach (var cell in _cells)
            {
                cell.SetSprite(null);
            }
            foreach (var item in value)
                if (item != null)
                    TryToAddItem(item);
        }
    }

    public static System.Action<ItemCollection> OnItemCollectionSelected;
    public static System.Action OnItemCollectionDeselected;

    public event System.Action<ItemInfo> OnItemAdded;
    public event System.Action<ItemInfo> OnItemRemoved;

    private void Awake()
    {
        _items = new ItemInfo[_cells.Length];
        RestartPanel.OnRestarted += () => Reset();
    }

    protected virtual void Start()
    {
        for (var i = 0; i < _cells.Length; i++)
        {
            _cells[i].index = i;
            _cells[i].OnCellSelected += (value) => SelectCell(value);
            _cells[i].OnItemAdded += (value) => TryToAddItem(value.Item2, value.Item1);
            _cells[i].OnClicked += (value) => UseItem(value);
        }
    }

    protected virtual void Update()
    {
        ReadInputs();
    }

    protected virtual void Reset()
    {
        Items = new ItemInfo[_cells.Length];
    }

    public ItemCollection() { }

    public virtual bool TryToAddItem(ItemInfo item, int cell = -1)
    {
        if (item == null || _items.Contains(item))
            return false;
        _itemsCount++;

        var freeCell = cell;
        if (freeCell == -1)
            freeCell = GetFreeCell(item);
        if (freeCell == -1)
            return false;
        Items[freeCell] = (item);
        _cells[freeCell].SetSprite(GameController.Instance.ItemsData.GetSprite(item.Name));
        OnItemAdded?.Invoke(item);
        return true;
    }

    public virtual bool TryToAddItem(ItemInfo item, bool value)
    {
        if (Items.Contains(item))
            return false;
        var freeCell = GetFreeCell(item);
        Items[freeCell] = (item);
        _cells[freeCell].SetSprite(GameController.Instance.ItemsData.GetSprite(item.Name));
        return value;
    }

    public virtual bool TryToRemoveItem(ItemInfo item)
    {
        if (_items.Length == 0)
            _items = new ItemInfo[_cells.Length];
        for (var i = 0; i < _items.Length; i++)
        {
            if (_items[i] == item)
            {
                _items[i] = null;
                _cells[i].SetSprite(null);
                OnItemRemoved?.Invoke(item);
                return true;
            }
        }
        return false;
    }

    protected virtual void SetItems(List<ItemInfo> items)
    {
        _items = new ItemInfo[_cells.Length];
        foreach (var cell in _cells)
            cell.SetSprite(null);
        foreach (var item in items)
            TryToAddItem(item);
    }

    protected virtual void SetItems(ItemInfo[] items)
    {
        _items = new ItemInfo[_cells.Length];
        foreach (var cell in _cells)
            cell.SetSprite(null);
        for (var i = 0; i < items.Length; i++)
            TryToAddItem(items[i], i);
    }

    protected void SelectCell(int cellNubder)
    {
        if (!_isSelected)
            return;
        _selectedCellIndx = cellNubder;
    }

    protected void SelectItem()
    {
        if (!_isSelected || _items[_selectedCellIndx] == null)
            return;
        if (ItemDragger.Instance.SelectedItem == null)
        {
            _cells[_selectedCellIndx].SetSprite(null);
            ItemDragger.Instance.StartDragging(_items[_selectedCellIndx]);
        }
    }

    protected virtual void UseItem(int cellNubder)
    {
        if (_items[cellNubder] != null && _items[cellNubder].Type == ItemType.Consumable)
        {
            _consumableInfo = (ConsumableInfo)_items[cellNubder];

            MethodInfo staticMethod = staticClassType.GetMethod(_consumableInfo.AbilityName);
            staticMethod.Invoke(null, new object[] { _consumableInfo.Value });

            TryToRemoveItem(_items[cellNubder]);
        }
    }

    protected virtual int GetFreeCell(ItemInfo itemInfo)
    {
        for (var i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].IsFree)
                return i;
        }
        return -1;
    }

    protected virtual void ReadInputs()
    {
        if (Input.GetMouseButton(0))
            SelectItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnItemCollectionSelected?.Invoke(this);
        _isSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnItemCollectionDeselected?.Invoke();
        _isSelected = false;
    }
}

