using System.Linq;
using UnityEngine;

public class NeutralItemsPanel : ItemCollection
{
    [SerializeField] private GameObject _panel;

    private Chest _chest;

    protected override void Start()
    {
        base.Start();
        Chest.OnChestOpened += (chest) => Open(chest);
        _panel.SetActive(false);
        OnItemAdded += (value) => _chest.SetItems(Items.ToList());
        OnItemRemoved += (value) => _chest.SetItems(Items.ToList());
    }

    public void Open(Chest chest)
    {
        _chest = chest;
        PlayerInventory.Instance.Show(true);
        _panel.SetActive(true);
        SetItems(chest.Items);
    }
}
