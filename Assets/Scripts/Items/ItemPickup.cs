using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;   // Item to put in the inventory if picked up
    private void Start()
    {
        var coll = GetComponent<SphereCollider>();
        coll.radius = radius;
    }
    // When the player interacts with the item
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    // Pick up the item
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        Inventory.Instance.Add(item);   // Add to inventory

        Destroy(gameObject);    // Destroy item from scene
    }
    private void OnTriggerStay(Collider ingameObj)
    {

        // If gamne object has item tag you can pick up this object
        if (ingameObj.transform.tag == "Player")
        {
            Debug.Log("Press Keyboard >E< or Joystick >X< to interact with: " + ingameObj.name);
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Call Interact (Pick up)
                Interact();
            }
        }

    }
}
