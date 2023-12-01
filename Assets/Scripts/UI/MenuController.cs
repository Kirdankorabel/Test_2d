using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartAllButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private TMP_Text _text;

    public static event System.Action OnAllRestarted;

    private void Awake()
    {
        _continueButton.onClick.AddListener(() => Close());
        _restartAllButton.onClick.AddListener(() => Restart());
        _newGameButton.onClick.AddListener(() => Restart());        
        _settingsButton.onClick.AddListener(() => _settingsPanel.SetActive(true));
        _quitButton.onClick.AddListener(() => Application.Quit());
        GameController.Instance.OnGameCompleted += () => _winPanel.gameObject.SetActive(true);
    }

    private void Start()
    {
        Open();
        _winPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_menuPanel.gameObject.active)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    private void Restart()
    {
        _winPanel.gameObject.SetActive(false);
        OnAllRestarted?.Invoke();
        Close();
    }

    private void Close()
    {
        _menuPanel.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void Open()
    {
        _menuPanel.gameObject.SetActive(true);
        _text.text = $"Level: {GameController.Instance.Level.ToString()}";
        Time.timeScale = 0f;
    }
}
