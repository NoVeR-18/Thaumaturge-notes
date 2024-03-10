using Attack;
using UnityEngine;

public class FirstPersonAttack : MonoBehaviour
{
    [SerializeField] private AttackBehaviour _attackBehaviour;

    private const KeyCode LeftMouseButton = KeyCode.Mouse0;

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(LeftMouseButton))
        {
            _attackBehaviour.PerformAttack();
        }
    }
}
