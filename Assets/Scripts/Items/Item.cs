using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemInfo _itemInfo;
    private SpriteRenderer _spriteRenderer;

    public ItemInfo ItemInfo => _itemInfo;
    public Sprite Sprite => GameController.Instance.ItemsData.GetSprite(Name);
    public string Name => _itemInfo.Name;
    public int Price => _itemInfo.Price;
    public ItemType Type => _itemInfo.Type;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PickUp()
    {

    }

    public void SetItemInfo(ItemInfo info)
    {
        _itemInfo = info;
        _spriteRenderer.sprite = Sprite;
    }
}

