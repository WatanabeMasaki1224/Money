using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private Transform _exitPoint;

    private EnemyBase _enemyBase;
    private Punch _punch;
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerController _playerController;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _punch= GetComponent<Punch>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        switch (_enemyBase.State)
        {
            case EnemyBase.EnemyState.Normal:
                NormalUpdate();
                break;

            case EnemyBase.EnemyState.Attack:
                AttackUpdate();
                break;

            case EnemyBase.EnemyState.Negotiation:
                NegotiationUpdate();
                break;

            case EnemyBase.EnemyState.Exit:
                ExitUpdate();
                break;
        }
    }

    private void NormalUpdate()
    {
        if (_playerController.CurrentState == PlayerController.PlayerState.Stealing
        && CanSeePlayer())
        {
            _enemyBase.ChangeState(EnemyBase.EnemyState.Attack);
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;

        // �������m�F
        if (direction.magnitude > _viewDistance)
            return false;

        // ���E�̊p�x���m�F
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > _viewAngle / 2f)
            return false;

        return true;
    }


    private void AttackUpdate()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        // �U�����͈ړ����Ȃ�
        if (_punch.IsAttacking)
            return;

        // �U���I����A�v���C���[�̕���������
        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }

        // ���E�̋�����艓����Βʏ��Ԃɖ߂�
        if (distance > _viewDistance)
        {
            if (_animator != null)
                _animator.SetBool("isWalking", false);
            _enemyBase.ChangeState(EnemyBase.EnemyState.Normal);

            return;
        }

        // �U�����͂������Ȃ�U��
        if (distance <= _attackRange)
        {
            if (_animator != null)
                _animator.SetBool("isWalking", false);

            _punch.Attack();
            return;
        }

        // �U�����͂��Ȃ���΃v���C���[�ɐڋ�
        Vector3 moveDirection = direction.normalized;

        if (_characterController != null)
        {
            _characterController.Move(
                moveDirection * _moveSpeed * Time.deltaTime);
        }

        if (_animator != null)
        {
            _animator.SetBool("isWalking", true);
        }
    }

    private void NegotiationUpdate()
    {
        // �����̍s��
    }

    private void ExitUpdate()
    {
        if(_exitPoint == null)
        {
            if(_animator != null)
                _animator.SetBool("isWalkig",false);
            
            return;
        }

        Vector3 direction = _exitPoint.position - transform.position;
        direction.y = 0f;

        //脱出口に到着したら削除
        if(direction.magnitude <= 0.2)
        {
            if(_animator != null)
                _animator.SetBool("isWalking",false);

            Destroy(gameObject);
            return;
        }

        direction.Normalize();
        transform.forward = direction;

        if (_characterController != null)
        {
            _characterController.Move(
                direction * _moveSpeed * Time.deltaTime
        );
        }

        if (_animator != null)
        {
            _animator.SetBool("isWalking", true);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        float halfAngle = _viewAngle / 2f;
        Vector3 forward = transform.forward;

        // ���E�̍��E�̒[
        Vector3 leftDirection =
            Quaternion.Euler(0f, -halfAngle, 0f) * forward;

        Vector3 rightDirection =
            Quaternion.Euler(0f, halfAngle, 0f) * forward;

        Gizmos.DrawLine(
            transform.position,
            transform.position + leftDirection * _viewDistance);

        Gizmos.DrawLine(
            transform.position,
            transform.position + rightDirection * _viewDistance);

        // ���E�̐�[���ʂŌ���
        Vector3 previousPoint =
            transform.position + leftDirection * _viewDistance;

        for (int i = 1; i <= 20; i++)
        {
            float angle = Mathf.Lerp(
                -halfAngle, halfAngle, i / 20f);

            Vector3 direction =
                Quaternion.Euler(0f, angle, 0f) * forward;

            Vector3 point =
                transform.position + direction * _viewDistance;

            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }
}
