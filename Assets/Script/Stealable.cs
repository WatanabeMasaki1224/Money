using UnityEngine;

public class Stealable : MonoBehaviour
{
    [SerializeField] private int _moneyAmount = 100;
    public bool IsStolen{get;private set;}

    public void CompleteSteal(PlayerController player)
    {
        if(IsStolen)
          return;
        IsStolen = true;
        player.AddMoney(_moneyAmount);
        Debug.Log("ぬすんだ");
        Destroy(gameObject);

    }
}
