using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MoveController : Singleton<MoveController>
{
    [SerializeField] private Rigidbody2D _rigidbody2;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterAudioController _characterAudioController;
    [SerializeField] private float _velocity = 5f;

    private Vector3 _direction;
    private Enemy _target;

    public readonly Vector3[] Directions = new Vector3[]
    {
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
        new Vector2(-1, 1),
        new Vector2(-1, 0),
        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
    };

    public Vector3 Direction { get; private set; }

    private void Awake()
    {
        _agent.speed = Player.Instance.Characteristics.speed;
    }

    void Update()
    {
        ReadInputs();
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetTarget(Enemy target)
    {
        _target = target;
    }

    public void Teleportation(Vector3 positon)
    {
        _agent.ResetPath();
        _agent.enabled = false;
        _target = null;
        transform.position = positon;
        new WaitForEndOfFrame();
        _agent.enabled = true;
    }

    private void Move()
    {
        if(Player.Instance.IsDead) 
            return;
        if (_direction != Vector3.zero)
        {
            _agent.SetDestination(transform.position + _direction);
            _target = null;
            _animator.SetBool("Idle", false);
            _characterAudioController.PlayStepAudio(true);
        }
        else if (_direction == Vector3.zero && _target != null && Vector3.Distance(transform.position, _target.transform.position) > Player.Instance.Characteristics.attackDist)
        {
            _characterAudioController.PlayStepAudio(true);
            _agent.SetDestination(_target.transform.position - (_target.transform.position - transform.position).normalized);
            _animator.SetBool("Idle", false);
        }
        else if ((_direction == Vector3.zero && _target == null )|| _agent.remainingDistance > 0.1f)
        {
            _agent.ResetPath();
            _animator.SetBool("Idle", true);
            _characterAudioController.PlayStepAudio(false);
        }
        _direction = Vector3.zero;
    }

    private void ReadInputs()
    {
        if (Input.GetKey(KeyCode.D))
            _direction = Directions[0];
        if (Input.GetKey(KeyCode.W))
            _direction = Directions[2];
        if (Input.GetKey(KeyCode.A))
            _direction = Directions[4];
        if (Input.GetKey(KeyCode.S))
            _direction = Directions[6];

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
            _direction = Directions[1];
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            _direction = Directions[3];
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            _direction = Directions[5];
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            _direction = Directions[7];
    }
}
