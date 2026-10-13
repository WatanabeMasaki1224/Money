using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewAngle = 90f;
    private EnemyBase _enemyBase;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
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
        if (CanSeePlayer())
        {
            Debug.Log("プレイヤーを視界内で発見");
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;

        // 距離を確認
        if (direction.magnitude > _viewDistance)
            return false;

        // 視界の角度を確認
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > _viewAngle / 2f)
            return false;

        return true;
    }


    private void AttackUpdate()
    {
        // 攻撃時の行動
    }

    private void NegotiationUpdate()
    {
        // 交渉中の行動
    }

    private void ExitUpdate()
    {
        // 退場時の行動
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        float halfAngle = _viewAngle / 2f;
        Vector3 forward = transform.forward;

        // 視界の左右の端
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

        // 視界の先端を弧で結ぶ
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
