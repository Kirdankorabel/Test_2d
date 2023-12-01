using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragger : Singleton<ItemDragger>
{
    public enum DraggingMode
    {
        Shop,
        BottomPanel,
        Inventory
    }

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _draggableItemImage;
    [SerializeField] private Vector3 _offset = new Vector3(51, 51);

    private bool _dragged;
    private ItemInfo _selectedItem;
    private ItemCollection _itemCollection;
    private ItemCollection _itemOwnerCollection;
    private Cell _itemOwnerCell;
    private Cell _cell;

    public bool Dragged => _dragged;

    public ItemInfo SelectedItem => _selectedItem;

    public event System.Action<ItemInfo> OnDraggingAborted;
    public event System.Action OnDraggingStarted;

    private void Start()
    {
        ItemCollection.OnItemCollectionSelected += (value) => _itemCollection = value;
        ItemCollection.OnItemCollectionDeselected += () => _itemCollection = null;
        Cell.OnUsed += (value) => _dragged = false;
        Cell.OnSelected += (value) => _cell = value;
        Cell.OnDeselected += (value) => _cell = _cell == value ? null : value;
    }

    public void StartDragging(ItemInfo item)
    {
        _itemOwnerCollection = _itemCollection;
        if (!_dragged) 
            StartCoroutine(DraggingCorutine(item));
    }

    private IEnumerator DraggingCorutine(ItemInfo item)
    {
        _dragged = true;
        _selectedItem = item;
        _draggableItemImage.gameObject.SetActive(true);
        _draggableItemImage.sprite = GameController.Instance.ItemsData.GetSprite(item.Name);
        OnDraggingStarted?.Invoke();

        while (_dragged && Input.GetMouseButton(0))
        {
            _draggableItemImage.transform.localPosition = (Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2f) / _canvas.scaleFactor + _offset;
            yield return null;
        }
        OnDraggingAborted?.Invoke(item);

        var seccess = true;
        if (_dragged && _itemOwnerCollection != null && _itemOwnerCollection.TryToRemoveItem(item))
        {
            if (_itemCollection != null && _cell != null && _cell.IsFree)
            {
                seccess = _itemCollection.TryToAddItem(item, _cell.index);
            }
            else if ((_itemCollection != null && _cell != null && !_cell.IsFree) || (_itemCollection != null && _cell == null))
            {
                seccess = _itemCollection.TryToAddItem(item);
            }
            else
            {
                ItemFactoty.Instance.CreateItem(item, GameController.Instance.GetPlayer.gameObject.transform.position + Vector3.left * 1.6f);
            }

            if (!seccess)
            {
                if(!_itemOwnerCollection.TryToAddItem(item)) ;
                ItemFactoty.Instance.CreateItem(item, GameController.Instance.GetPlayer.gameObject.transform.position + Vector3.left * 1.6f);
            }
        }
        else if (_dragged || _selectedItem.Type != ItemType.Consumable)
        {
            //ItemFactoty.Instance.CreateItem(item, LevelController.Instance.GetPlayer.gameObject.transform.position + Vector3.left * 1.6f);
            _itemOwnerCollection.TryToAddItem(item, true);//TODO
        }
        _draggableItemImage.gameObject.SetActive(false);
        _selectedItem = null;
        _dragged = false;

        yield return null;
    }
}
