using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingObject : MonoBehaviour
{
    public static event System.Action<InteractingObject> OnInteractingObjectActiveted;
    public static event System.Action<InteractingObject> OnInteractingObjectDeactiveted;

    public virtual void Use(GameObject user)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<TriggerController>() != null)
            OnInteractingObjectActiveted?.Invoke(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TriggerController>() != null)
            OnInteractingObjectDeactiveted?.Invoke(this);
    }
}
