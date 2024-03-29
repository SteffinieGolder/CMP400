using UnityEngine;

//Script which sets up in game items. Requires a rigid body 2D. 

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    //The item's scriptable object data. 
    public ItemData data;

    [HideInInspector] public Rigidbody2D rb2d;

    private void Awake()
    { 
        //Initialise rigid body. 
        rb2d = GetComponent<Rigidbody2D>();
    }

    //Checks if player has collided which the collectable.
    //Adds item to the inventory and destroys it from the environment.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            player.inventoryManager.Add(player.inventoryManager.backpack.inventoryName, this);

            if (data.itemName == "Hoe")
            {
                //Show Dialogue lines.
                GameManager.instance.uiManager.SetDialogueData(player.charData.GetDialogueGroup(data.itemFoundIndex).dialogueLines,
                    player.charData.GetDialogueGroup(data.itemFoundIndex).expressionTypes);
            }

            Destroy(this.gameObject);
        }
    }
}
