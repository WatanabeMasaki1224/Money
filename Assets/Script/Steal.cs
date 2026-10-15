using UnityEngine;

public class Steal : MonoBehaviour
{
    [SerializeField] private float _stealDistance = 2f;
    [SerializeField] private float _stealTime = 3f;
    [SerializeField] private float _rayHeight = 1f;

    private PlayerController _playerController;
    private Stealable _target;
    private float _elapsedTime;

    public bool IsStealing{get;private set;}

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void StartSteal()
    {
        if (IsStealing)
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
        IsStealing = true;
        _playerController.SetStealing(true);

        Debug.Log("盗み開始");
    }

    private void Update()
    {
        if (!IsStealing)
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
        if (!IsStealing)
            return;

        Debug.Log("盗み中断");
        EndSteal();
    }

    private void EndSteal()
    {
        IsStealing = false;
        _target = null;
        _elapsedTime = 0f;

        _playerController.SetStealing(false);
    }
}
