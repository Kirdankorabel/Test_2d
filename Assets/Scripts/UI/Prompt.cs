using UnityEngine;

public class Prompt : MonoBehaviour
{
    private void Awake()
    {
        InteractingObject.OnInteractingObjectActiveted += (value) => gameObject.SetActive(true);
        InteractingObject.OnInteractingObjectDeactiveted += (value) => gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
            gameObject.SetActive(false);
    }
}
