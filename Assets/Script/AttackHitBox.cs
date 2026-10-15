using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] EnemyBase _enemyBase;

    private void OnTriggerEnter(Collider other)
    {
        // ƒvƒŒƒCƒ„[‚ÌUŒ‚
        if (_playerController != null)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            if (enemy != null)
                enemy.TakeDamage(_playerController.AttackPower);
        }

        // “G‚ÌUŒ‚
        if (_enemyBase != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                player.TakeDamage(_enemyBase.AttackPower);
        }
    }
}
