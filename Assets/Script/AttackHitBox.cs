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
                enemy.TakeDamage(_playerController.AttackPower);
        }

        // �G�̍U��
        if (_enemyBase != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                player.TakeDamage(_enemyBase.AttackPower);
            
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if(enemy != null)
                enemy.TakeDamage(_enemyBase.AttackPower);
        }
    }
}
