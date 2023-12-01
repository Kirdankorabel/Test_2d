using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour, IDamageTooker
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;

    private int _health;

    public int Health
    {
        get
        {
            return _health > 0 ? _health : 0;
        }
    }

    public event System.Action OnDeadth;
    public UnityEvent<float> OnHealthChanged;

    private void Awake()
    {
        RestartPanel.OnRestarted += () => Reset();
        GameController.Instance.OnLevelCompleted += () => Reset();
        Player.Instance.OnCharacteristicsUpdated += (value) => OnHealthChanged?.Invoke((float)_health / Player.Instance.Characteristics.HP);
    }

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        _health = Player.Instance.Characteristics.HP;

        _animator.SetBool("IsDead", false);
        OnHealthChanged?.Invoke(1);
    }

    public void TookDamage(int damage)
    {
        if (damage > 0)
        {
            damage -= Player.Instance.Characteristics.armor;
            damage = damage > 0? damage : 0;
        }
        _health -= damage;
        if (_health > Player.Instance.Characteristics.HP) 
        {
            _health = Player.Instance.Characteristics.HP;
        }
        if (_health <= 0)
        {
            OnDeadth?.Invoke();
            _animator.SetBool("IsDead", true);
        }
        OnHealthChanged?.Invoke((float)_health / Player.Instance.Characteristics.HP);
    }
}
