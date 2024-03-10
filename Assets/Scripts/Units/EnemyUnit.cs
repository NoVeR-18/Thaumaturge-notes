using Enums;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField] private EnemyUnitType _enemyUnitType;

    public EnemyUnitType EnemyUnitType => _enemyUnitType;
}