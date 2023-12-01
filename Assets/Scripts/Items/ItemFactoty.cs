using UnityEngine;
using UnityEngine.Pool;

public class ItemFactoty : Singleton<ItemFactoty>
{
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<Item> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Item>(
            InstantiateItem,
            OnGet,
            OnReleas,
            OnDestroyElement,
            false,
            _defaultCapacity,
            _maxSize);
    }
    private void OnApplicationQuit()
    {
        _pool.Dispose();
    }

    public Item CreateItem(ItemInfo itemInfo, Vector3 position)
    {
        var item = _pool.Get();
        item.SetItemInfo(itemInfo);
        item.transform.parent = transform;
        item.transform.position = position;
        return item;
    }

    public void DestroyItem(Item item)
    {
        _pool.Release(item);
    }

    #region pool methods
    private Item InstantiateItem()
    {
        var item = Instantiate(_itemPrefab);
        return item;
    }

    private void OnGet(Item gameObject)
    {
        gameObject.gameObject.SetActive(true);
        gameObject.transform.parent = transform;
    }
    private void OnReleas(Item gameObject)
    {
        gameObject.gameObject.SetActive(false);
    }
    private void OnDestroyElement(Item gameObject) { }
    #endregion
}
