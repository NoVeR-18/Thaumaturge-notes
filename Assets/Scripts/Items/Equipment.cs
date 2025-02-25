using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipments")]
public class Equipment : Item
{

    public EquipmentSlot equipSlot;
    public int armorModifier;
    public int damageModifier;
    public GameObject objectPrefab;

    public override void Use()
    {
        //EquipmentManager.instance.Equip(this);	// Equip
        //RemoveFromInventory();	// Remove from inventory
    }

}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }
