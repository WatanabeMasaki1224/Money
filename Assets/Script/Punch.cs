using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _attackHitbox;
    [SerializeField] private float _attackCooldDown = 2f;
    private float _nextAttackTime;
    public bool IsAttacking { get;private set; }


    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_attackHitbox != null)
            _attackHitbox.enabled = false;
    }

    public void Attack()
    {
        if (IsAttacking)
            return;

        if (Time.time < _nextAttackTime)
            return;

        Debug.Log("UŒ‚ŠJŽn");
        IsAttacking = true;
        _animator.SetTrigger("Attack");
    }

    public void EnableAttackHitbox()
    {
        if (_attackHitbox != null)
            _attackHitbox.enabled = true;
    }

    public void DisableAttackHitbox()
    {
        if (_attackHitbox != null)
            _attackHitbox.enabled = false;
        Debug.Log("UŒ‚I—¹");
        IsAttacking = false;
        _nextAttackTime = Time.time + _attackCooldDown;
        _animator.ResetTrigger("Attack");
    }
}
