using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractingObject
{
    [SerializeField] private Transform _otherTransform;
    [SerializeField] private float _teleportationTime;

    public event System.Action OnPlayerTeleported;

    public void SetTransform(Transform transform)
    {
        _otherTransform = transform;
    }

    public override void Use(GameObject user)
    {
        StartCoroutine(TeleportationCorutine(user));
    }

    private IEnumerator TeleportationCorutine(GameObject user)
    {
        float time = _teleportationTime;
        while(Input.GetKey(KeyCode.E) && time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if(time < 0)
            user.GetComponent<MoveController>().Teleportation(_otherTransform.position);
        OnPlayerTeleported?.Invoke();
    }
}
