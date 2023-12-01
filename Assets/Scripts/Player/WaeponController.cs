using System.Collections;
using UnityEngine;

public class WaeponController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _camera;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private CharacterAudioController _characterAudioController;

    private Enemy _enemy;
    private Characteristics _characteristics;
    private Vector3 _mousePosition;
    private Vector3 _position;
    private float _lastAttackTime = float.MinValue;

    private void Awake()
    {
        Player.Instance.OnCharacteristicsUpdated += (value) => UpdateCharacteristics(value);
    }

    void Update()
    {
        ReadInputs();
    }

    private void UpdateCharacteristics(Characteristics characteristics)
    {
        _characteristics = characteristics;
    }

    private void ReadInputs()
    {
        if (Player.Instance.IsDead)
            return;
        if (Input.GetMouseButton(0))
        {
            _mousePosition = Input.mousePosition;
            _position = _camera.ScreenToWorldPoint((new Vector3(_mousePosition.x, _mousePosition.y, 10)));

            _enemy = GameController.Instance.GetNearestEnemy(_position);
            if (ReadyToAttackCondition())
            {
                Attack(_enemy);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _enemy = GameController.Instance.GetNearestEnemy(transform.position);
            if (ReadyToAttackCondition())
            {
                Attack(_enemy);
            }
            else if (!_enemy.IsDead)
            {
                _moveController.SetTarget(_enemy);
                StartCoroutine(RunToEnemyCorutine());
            }

        }
    }

    private void Attack(Enemy enemy)
    {
        _animator.SetBool("Attack", true);
        _characterAudioController.PlayAttackAudio();
        _lastAttackTime = Time.time;
        _characterAudioController.PlayStepAudio(false);

        StartCoroutine(AttackCorutine(_characteristics.timeToDamage));
    }

    private bool ReadyToAttackCondition()
    {
        if (_enemy != null && !_enemy.IsDead && Time.time - _lastAttackTime > _characteristics.cooldown && Vector3.Distance(_position, _enemy.transform.position) < _characteristics.attackDist)
            return true;
        else 
            return false;
    }

    private IEnumerator RunToEnemyCorutine()
    {
        while (Vector3.Magnitude(_enemy.transform.position - transform.position) > _characteristics.attackDist)
        {
            yield return null;
        }

        if (Time.time - _lastAttackTime > _characteristics.cooldown)
            Attack(_enemy);
        yield break;
    }

    private IEnumerator AttackCorutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (Vector3.Magnitude(_enemy.transform.position - transform.position) < _characteristics.attackDist)
            _enemy.TookDamage(_characteristics.damage);
        _animator.SetBool("Attack", false);
    }
}
