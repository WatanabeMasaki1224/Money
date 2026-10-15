using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
     [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private PlayerController _playerController;

    private void Update()
    {
        _moneyText.text = $"Money : {_playerController.Money}";
    }
}
