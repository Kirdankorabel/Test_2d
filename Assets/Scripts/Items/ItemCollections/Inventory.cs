using UnityEngine;

public class Inventory : ItemCollection
{
    [SerializeField] private TriggerController _triggerController;

    protected override void Start()
    {
        base.Start();
        _triggerController.OnItemPickUp += (value) => TryToAddItem(value);
    }

    protected override void Update()
    {
        base.Update();
    }
}
