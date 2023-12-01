using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image _image;
    private bool _isActive;
    private bool _isFree = true;

    public int index;

    public bool IsFree => _isFree;

    public event System.Action<int> OnCellSelected;
    public event System.Action<(int, ItemInfo)> OnItemAdded;
    public event System.Action<int> OnClicked;

    public static event System.Action<Cell> OnSelected;
    public static event System.Action<Cell> OnDeselected;
    public static event System.Action<Cell> OnUsed;

    public void SetSprite(Sprite sprite)
    {
        if(sprite == null )
        {
            _isFree = true;
            _image.gameObject.SetActive(false);
        }
        else
        {
            _isFree = false;
            _image.sprite = sprite;
            _image.gameObject.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isActive = true;
        OnCellSelected?.Invoke(index);
        OnSelected?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnDeselected?.Invoke(this);
        _isActive = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(index);
        OnUsed?.Invoke(this);
    }
}
