using UnityEditor.MPE;
using UnityEngine;

public class PickPocket : MonoBehaviour
{
    [SerializeField] private float _pickpocketDistance= 2f;
    [SerializeField] private float _pickpocketTime = 2f;
    [SerializeField] private float _rayHeight = 1f;

    private PlayerController _playerController;
    private EnemyBase _target;
    private float _elapsedTime;
    private StealHitBox _stealHitBox;
    

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stealHitBox = GetComponentInChildren<StealHitBox>();
    }

    public bool StartPickpocket()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Normal)
            return false;

        _target = _stealHitBox.GetEnemy();

        if (_target == null )
            return false;

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
            CompletePickpocket();
        }
    }

    private void CompletePickpocket()
    {
        int attemptCount = _target.PickpocketCount +1;
        float fallureRate = 1f;

        if(attemptCount == 1)
            fallureRate = 0.2f;
        else if(attemptCount == 2)
            fallureRate = 0.5f;
        else if(attemptCount == 3)
            fallureRate = 0.8f;

        int randomValue = Random.Range(0,100);
        bool failed = randomValue < fallureRate * 100f;
        //成功・失敗にかかわらず数を増やす
        _target.AddPickpocketCount();

        if(failed)
        {
            _target.ChangeState(EnemyBase.EnemyState.Attack);
            EndPickpocket();
            return;
        }

        int amount = _target.PickpocketAmount;
        _playerController.AddMoney(amount);
        EndPickpocket();
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
