using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class Consumable : Item
{

    public int healthGain;
    public int hungerGain;
    public int thirstyGain;

    public override void Use()
    {
        base.Use();
    }

}
