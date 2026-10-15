using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        Normal,       // 通常
        Attack,       // 攻撃
        Negotiation,  // 交渉中
        Exit          // 交渉成功後、退場
    }

    [SerializeField] private int _hp = 5;
    [SerializeField] private EnemyState _state = EnemyState.Normal;
    [SerializeField] private int _attackPower = 10;
    public int AttackPower => _attackPower;
    public EnemyState State => _state;

    public void ChangeState(EnemyState state)
    {
        _state = state;
    }
    public void TakeDamage(int damage)
    {
        _hp -= damage;
        Debug.Log($"{gameObject.name} が {damage} ダメージを受けた。残りHP：{_hp}");

        if(_hp <= 0)
        {
            _hp = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
