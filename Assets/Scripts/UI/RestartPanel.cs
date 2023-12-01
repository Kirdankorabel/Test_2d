using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;

    public static event System.Action OnRestarted;

    private void Awake()
    {
        Player.Instance.OnLose += () => gameObject.SetActive(true);
        _restartButton.onClick.AddListener(() => Restart());
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Restart()
    {
        gameObject.SetActive(false);
        OnRestarted?.Invoke();
    }
}
