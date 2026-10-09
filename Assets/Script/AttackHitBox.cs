using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy == null)
            return;

        enemy.TakeDamage(_playerController.AttackPower);
    }
}
