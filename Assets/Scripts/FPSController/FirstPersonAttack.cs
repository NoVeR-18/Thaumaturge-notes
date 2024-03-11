using Attack;
using UnityEngine;

public class FirstPersonAttack : MonoBehaviour
{
    [SerializeField] private AttackBehaviour _attackBehaviour;
    [SerializeField] private Transform _inventoryView;

    private const KeyCode LeftMouseButton = KeyCode.Mouse0;
    private const KeyCode InventoryShow = KeyCode.I;

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(LeftMouseButton))
        {
            _attackBehaviour.PerformAttack();
        }
        if (UnityEngine.Input.GetKeyDown(InventoryShow))
        {
            if (_inventoryView.gameObject.activeSelf)
                _inventoryView.gameObject.SetActive(false);
            else
                _inventoryView.gameObject.SetActive(true);
        }
    }
}
