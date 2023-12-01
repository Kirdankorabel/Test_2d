using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageTooker
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _target;
    [SerializeField] private Characteristics _characteristics;
    [SerializeField] private AngryController _angryController;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterAudioController _characterAudioController;
    protected int _health = 5;

    private float _lastAttackTime = float.MinValue;
    private bool _isdead;
    private Vector3 _startPositon;
    private bool _isWalk;

    public bool IsDead => _isdead;
    public Characteristics Characteristics => _characteristics;

    public event System.Action OnDeadth;

    private void Awake()
    {
        _startPositon = transform.position;
    }

    void Start()
    {
        SetTarget(GameController.Instance.GetPlayer);
    }

    void Update()
    {        
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (_target != null && !IsDead && _angryController.IsAngry)
            _agent.SetDestination(_target.transform.position - (_target.transform.position - transform.position).normalized);
        else if (!IsDead)
            _agent.SetDestination(_startPositon);
        if(_isWalk && _agent.remainingDistance < 0.1f || !_isWalk && _agent.enabled && _agent.remainingDistance > 0.1f)
        {
            _isWalk = !_isWalk;
            _animator.SetBool("Walk", _isWalk);
            _characterAudioController.PlayStepAudio(_isWalk);
        }
    }

    public void TookDamage(int damage)
    {
        damage -= _characteristics.armor;
        if(damage > 0)
            _health -= damage;
        if (_health < 0 && !IsDead)
        {
            OnDeadth?.Invoke();
            _isdead = true;
            _animator.SetTrigger("Dead");
            StopAllCoroutines();
            _agent.enabled = false;
            _characterAudioController.PlayStepAudio(false);
        }
        else
        {
            _animator.SetTrigger("TookDamage");
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SetCharacteristics(Characteristics characteristics)
    { 
        _characteristics = characteristics;
        _health = characteristics.HP;
        _agent.speed = characteristics.speed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MoveController>() && !Player.Instance.IsDead && !IsDead && Time.time - _lastAttackTime > _characteristics.cooldown)
        {
            _characterAudioController.PlayAttackAudio();
            _lastAttackTime = Time.time;
            _animator.SetInteger("Attack", 1);
            StartCoroutine(AttackCorutine(collision.gameObject, _characteristics.timeToDamage));
        }
    }

    private IEnumerator AttackCorutine(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        if (Vector3.Magnitude(target.transform.position - transform.position) < _characteristics.attackDist)
            target.GetComponent<IDamageTooker>().TookDamage(_characteristics.damage);
    }

    private void Reset()
    {
        _health = _characteristics.HP;
    }
}

public enum EnemyType
{
    Sceletone,
    Goblin
}
