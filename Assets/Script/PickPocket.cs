using UnityEngine;

public class PickPocket : MonoBehaviour
{
    [SerializeField] private float _pickpocketDistance= 2f;
    [SerializeField] private float _pickpocketTime = 2f;
    [SerializeField] private float _rayHeight = 1f;

    private PlayerController _playerController;
    private EnemyBase _target;
    private float _elapsedTime;
    

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public bool StartPickpocket()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Normal)
            return false;

        Vector3 origin = transform.position + Vector3.up * _rayHeight;

        // プレイヤーの正面にRayを飛ばす
        if (!Physics.Raycast(
            origin,
            transform.forward,
            out RaycastHit hit,
            _pickpocketDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            return false;
        }

        // 当たったCollider自身、または親からEnemyBaseを取得
        EnemyBase enemy = hit.collider.GetComponentInParent<EnemyBase>();

        if (enemy == null || enemy.State != EnemyBase.EnemyState.Normal)
            return false;

        _target = enemy;
        _elapsedTime = 0f;

        _playerController.SetState(
            PlayerController.PlayerState.Pickpocketing);

        Debug.Log("スリ開始");
       
        return true;
    }

    private void Update()
    {
        if (_playerController.CurrentState !=
            PlayerController.PlayerState.Pickpocketing)
            return;

        if (_target == null ||
            _target.State != EnemyBase.EnemyState.Normal)
        {
            CancelPickpocket();
            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _pickpocketTime)
        {
            // このNPCからスリを試した回数を増やす
            _target.AddPickpocketCount();

            // スリで盗める金額を獲得する
            int amount = _target.PickpocketAmount;
            _playerController.AddMoney(amount);

            Debug.Log(
                $"スリ成功：{amount}円、試行回数：{_target.PickpocketCount}");

            EndPickpocket();
        }
    }

    public void CancelPickpocket()
    {
        if(_playerController.CurrentState != PlayerController.PlayerState.Pickpocketing)
        return;

        Debug.Log("スリ中断");
        EndPickpocket();
    }

    public void EndPickpocket()
    {
        _target = null;
        _elapsedTime = 0f;
        _playerController.SetState(PlayerController.PlayerState.Normal);
    }
}
