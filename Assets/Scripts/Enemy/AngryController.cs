using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryController : MonoBehaviour
{
    private bool _isAngry;
    public bool IsAngry => _isAngry;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isAngry = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isAngry = false;
    }
}
