using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] EnemyBase _enemyBase;

    private void OnTriggerEnter(Collider other)
    { 
        Debug.Log($"攻撃判定に接触：{other.name}");
        // �v���C���[�̍U��
        if (_playerController != null)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null && !enemyAI.IsAlly)
                {
                    // プレイヤーに殴られた敵も、クールタイム後にプレイヤーへ狙いを切り替える。
                    enemyAI.SetEnemyCombatTarget(_playerController.transform);
                }

                enemy.TakeDamage(_playerController.AttackPower);
            }
        }

        // �G�̍U��
        if (_enemyBase != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                player.TakeDamage(_enemyBase.AttackPower);

            EnemyBase target = other.GetComponent<EnemyBase>();

            if (target != null)
            {
                EnemyAI attackerAI = _enemyBase.GetComponent<EnemyAI>();
                EnemyAI targetAI = target.GetComponent<EnemyAI>();

                // 味方NPCが敵を殴ったら、殴られた敵のターゲットを味方NPCにする
                if (attackerAI != null &&
                    attackerAI.IsAlly &&
                    targetAI != null &&
                    !targetAI.IsAlly)
                {
                    targetAI.SetEnemyCombatTarget(_enemyBase.transform);
                }

                target.TakeDamage(_enemyBase.AttackPower);
            }
        }
    }
}
