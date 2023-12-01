using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Canvas _canvas;

    private RectTransform _rectTransform;

    private ItemCollection _itemCollection;
    private StringBuilder stringBuilder = new StringBuilder();
    private ItemInfo _itemInfo;
    private Vector3 _position;

    private void Awake()
    {
        ItemCollection.OnItemCollectionSelected += (value) => _itemCollection = value;
        Cell.OnSelected += (value) => ShowInfo(value);
        Cell.OnDeselected += (value) => _infoPanel.SetActive(false);
        ItemDragger.Instance.OnDraggingStarted += () => _infoPanel.SetActive(false);
    }

    void Start()
    {
        _infoPanel.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            _infoPanel.SetActive(false);
        }
    }

    private void ShowInfo(Cell cell)
    {
        if (cell == null || ItemDragger.Instance.Dragged || _itemCollection == null || cell.index > _itemCollection.Items.Length)
            return;
        _itemInfo = _itemCollection.Items[cell.index];
        if (_itemInfo == null || string.IsNullOrEmpty(_itemInfo.Name))
            return;
        _infoPanel.SetActive(true);
        _text.text = _itemInfo.GetInfo();
        _position = (Input.mousePosition - new Vector3(Screen.width, Screen.height) / 2f) / _canvas.scaleFactor;
        if(_position.x <= 0) 
            _position += new Vector3(_rectTransform.sizeDelta.x / 4, 0);
        else 
            _position -= new Vector3(_rectTransform.sizeDelta.x / 4, 0); 
        if (_position.y <= 0)
            _position += new Vector3(0, _rectTransform.sizeDelta.y / 4);
        else
            _position -= new Vector3(0, _rectTransform.sizeDelta.y / 4);
        _rectTransform.anchoredPosition = _position;
    }
}
