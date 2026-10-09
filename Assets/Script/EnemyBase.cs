using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int _hp = 5;

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
