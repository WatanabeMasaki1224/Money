using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
[SerializeField] private StealHitBox _stealHitBox;
[SerializeField] private Camera _camera;
[SerializeField] private Vector3 _offset = new Vector3(0f, 1f, 0f);

private void Update()
{
    Stealable stealable = _stealHitBox.GetStealable();
    EnemyBase enemy = _stealHitBox.GetEnemy();

    Transform target = null;
    string message = "";

    if (stealable != null)
    {
        target = stealable.transform;
        message = "右クリックで盗む";
    }
    else if (enemy != null)
    {
        target = enemy.transform;
        message = "右クリックでスリ";
    }

    if (target == null)
    {
        _text.gameObject.SetActive(false);
        return;
    }

    Vector3 screenPosition = _camera.WorldToScreenPoint(target.position + _offset);

    if (screenPosition.z <= 0f)
    {
        _text.gameObject.SetActive(false);
        return;
    }

    _text.gameObject.SetActive(true);
    _text.text = message;
    _text.rectTransform.position = screenPosition;
}
}
