using UnityEngine;

public class Negotiation : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private float _negotiationRange = 2f;

    private PlayerController _playerController;
    private EnemyBase _targetEnemy;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _dialoguePanel.SetActive(false);
        _choicePanel.SetActive(false);
    }

    public void StartNegotiation()
    {
        if (_playerController.CurrentState != PlayerController.PlayerState.Normal)
            return;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            _negotiationRange
        );

        foreach (Collider collider in colliders)
        {
            EnemyBase enemy = collider.GetComponentInParent<EnemyBase>();

            if (enemy == null)
                continue;

            if (enemy.State != EnemyBase.EnemyState.Normal)
                continue;

            _targetEnemy = enemy;
            _playerController.SetState(PlayerController.PlayerState.Negotiating);
            _targetEnemy.ChangeState(EnemyBase.EnemyState.Negotiation);

            _dialoguePanel.SetActive(true);
            _choicePanel.SetActive(true);

            return;
        }
    }

    public void EndNegotiation()
    {
        if (_targetEnemy != null &&
            _targetEnemy.State == EnemyBase.EnemyState.Negotiation)
        {
            _targetEnemy.ChangeState(EnemyBase.EnemyState.Normal);
        }

        _targetEnemy = null;

        _dialoguePanel.SetActive(false);
        _choicePanel.SetActive(false);

        _playerController.SetState(PlayerController.PlayerState.Normal);
    }

    public void OnForgiveButton()
{
    Debug.Log("見逃してを選択した");
}

public void OnAttackOtherButton()
{
    Debug.Log("敵を攻撃してを選択した");
}

public void OnNothingButton()
{
    Debug.Log("なんでもないを選択した");
    EndNegotiation();
}
}
