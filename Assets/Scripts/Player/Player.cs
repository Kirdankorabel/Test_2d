using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Characteristics _characteristics;

    private InteractingObject _interactingObject;
    private Vector3 _startPosition;
    private bool _isDead;

    public bool IsDead => _isDead;
    public PlayerInventory PlayerInventory => _inventory;
    public Characteristics Characteristics => _characteristics;
    public HealthController HealthController => _healthController;

    public event System.Action OnLose;
    public event System.Action<Characteristics> OnCharacteristicsUpdated;

    private void Awake()
    {
        InteractingObject.OnInteractingObjectActiveted += (value) => _interactingObject = value;
        InteractingObject.OnInteractingObjectDeactiveted += (value) => _interactingObject = _interactingObject == value ? null : value;
        _healthController.OnDeadth += () => Lose();
        RestartPanel.OnRestarted += () => Reset();
        MenuController.OnAllRestarted += () => Reset();
        _inventory.OnItemAdded += () => UpdateCharacteristics();
    }

    private void Start()
    {
        _startPosition = transform.position;
        UpdateCharacteristics();
    }

    private void Update()
    {
        if (_interactingObject != null && Input.GetKeyDown(KeyCode.E))
            _interactingObject.Use(gameObject);
    }

    private void Reset()
    {
        _moveController.Teleportation(_startPosition);
        _isDead = false;
    }

    private void UpdateCharacteristics()
    {
        _characteristics = new Characteristics(_inventory.Equipment);
        OnCharacteristicsUpdated?.Invoke(_characteristics);
    }

    private void Lose()
    {
        _isDead = true;
        OnLose?.Invoke();
    }

}
