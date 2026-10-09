using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] float _jumpPower = 7f;
    [SerializeField] float _gravity = -20f;
    [SerializeField] Collider _attackHitbox;

    private Vector2 _moveInput;
    private float _verticalVelocity;
    private bool _isGrounded;
    private bool _jumpRequested;
    private bool _isAttacking;

    private CharacterController _characterController;
    private Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        if(_attackHitbox != null)
        _attackHitbox.enabled = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _jumpRequested = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!context.performed || _isAttacking)
        return;

        _isAttacking = true;
        _animator.SetTrigger("Attack");
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
        _animator.SetBool("isWalking",moveDirection.sqrMagnitude>0.01f );

        if (moveDirection != Vector3.zero)
            transform.forward = moveDirection;

        if (_jumpRequested)
        {
            if (_isGrounded)
                _verticalVelocity = _jumpPower;

            _jumpRequested = false;
        }

        if (_isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);

        _isGrounded = _characterController.isGrounded;
    }

    public void EnableAttackHitbox()
    {
        Debug.Log("攻撃開始");
        _attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
         Debug.Log("攻撃終了");
        _attackHitbox.enabled = false;
        _isAttacking = false;
    }
}
