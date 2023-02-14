using UnityEngine;

//Script which controls collectables in game. 

//Requires the item scriptable object which holds this collectable's data (name, icon etc). 
[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    //Checks if player has collided which the collectable.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            //Adds this item's data to the player's inventory and removes object from game. 
            Item item = GetComponent<Item>();

            if(item !=null)
            {
                player.inventory.Add("Backpack", item);
                Destroy(this.gameObject);
            }
        }
    }
}

