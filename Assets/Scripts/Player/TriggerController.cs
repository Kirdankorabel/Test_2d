using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public event System.Action<ItemInfo> OnItemPickUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Item>())
        {
            var item = collision.gameObject.GetComponent<Item>();
            item.PickUp();
            OnItemPickUp?.Invoke(item.ItemInfo);
            item.gameObject.SetActive(false);
            ItemFactoty.Instance.DestroyItem(item);
        }
        if (collision.gameObject.GetComponent<InteractingObject>())
        {
            var interactingObject = collision.gameObject.GetComponent<InteractingObject>();
        }
    }
}
