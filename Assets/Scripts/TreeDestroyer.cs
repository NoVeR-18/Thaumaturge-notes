using Interfases;
using UnityEngine;

public class TreeDestroyer : MonoBehaviour, IDamageable
{
    [SerializeField] private int _countToDestroy = 3;
    //[SerializeField] private int _dropedLog = 3;

    public void ApplyDamage(float damage)
    {
        if (_countToDestroy > 0)
            _countToDestroy--;
        else
        {
            Destroy(gameObject);
        }
    }
}
