using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] int _attackPower = 1;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] private int _hp = 100;

    private Vector2 _moveInput;
    public int AttackPower => _attackPower;

    private CharacterController _characterController;
    private Animator _animator;
    private Punch _punch;
    private Steal _steal;

public bool IsStealing { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _punch = GetComponent<Punch>();
        _steal = GetComponent<Steal>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed  && !IsStealing)
            _punch.Attack();
    }

    public void OnSteal(InputAction.CallbackContext context)
{
    if (_steal == null)
        return;

    if (context.started)
        _steal.StartSteal();

    if (context.canceled)
        _steal.CancelSteal();
}

    private void Update()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * _moveInput.y
                              + cameraRight * _moveInput.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        
        if (moveDirection != Vector3.zero)
            transform.forward = moveDirection;

        if (IsStealing)
        {
            moveDirection = Vector3.zero;

        }
        _animator.SetBool("isWalking",moveDirection.sqrMagnitude>0.01f );

        _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;

        Debug.Log($"プレイヤーが {damage} ダメージを受けた。残りHP：{_hp}");

        if (_hp <= 0)
        {
            _hp = 0;
            Debug.Log("プレイヤーが倒れた");
        }
    }

    public void SetStealing(bool value)
    {
        IsStealing = value;
    }

    public void AddMoney(int amount)
    {
        _hp += amount;
        Debug.Log($"お金を盗んだ｛amount｝");
    }
}
