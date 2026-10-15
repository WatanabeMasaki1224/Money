using UnityEngine;

public class Steal : MonoBehaviour
{
    [SerializeField] private float _stealDistance = 2f;
    [SerializeField] private float _stealTime = 3f;
    [SerializeField] private float _rayHeight = 1f;

    private PlayerController _playerController;
    private Stealable _target;
    private float _elapsedTime;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void StartSteal()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Normal)
            return;

        Vector3 origin = transform.position + Vector3.up * _rayHeight;

        // プレイヤーの正面にRayを飛ばす
        if (!Physics.Raycast(
            origin,
            transform.forward,
            out RaycastHit hit,
            _stealDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            return;
        }

         // 当たったCollider自身、または親からStealableを取得
        _target = hit.collider.GetComponentInParent<Stealable>();

        if (_target == null || _target.IsStolen)
        {
            _target = null;
            return;
        }

        _elapsedTime = 0f;
        _playerController.SetState(PlayerController.PlayerState.Stealing);

        Debug.Log("盗み開始");
    }

    private void Update()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Stealing)
            return;

        if (_target == null || _target.IsStolen)
        {
            CancelSteal();
            return;
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _stealTime)
        {
            _target.CompleteSteal(_playerController);
            EndSteal();
        }
    }

    public void CancelSteal()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Stealing)
            return;

        Debug.Log("盗み中断");
        EndSteal();
    }

    private void EndSteal()
    {
        _target = null;
        _elapsedTime = 0f;
        _playerController.SetState( PlayerController.PlayerState.Normal);
    }
}
