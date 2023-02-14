using UnityEngine;

//Script which controls player interactions. 

public class Player : MonoBehaviour
{
    //Player inventory object.
    public Inventory inventory;

    private void Awake()
    {
        //Initialise inventory with 24 slots. 
        inventory = new Inventory(24);
    }

    private void Update()
    {
        //Detect space bar press - player wants to interact with ground tile. 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Convert player world position to int for use with tile map. 
            Vector3Int position = new Vector3Int((int)transform.position.x,
                (int)transform.position.y, 0);

            //Check if tile map position contains an interactable tile. 
            if(GameManager.instance.tileManager.IsInteractable(position))
            {
                //Set that tile to its interacted version. 
                GameManager.instance.tileManager.SetInteracted(position);
            }
        }
    }

    //Function which allows player to drop inventory items. 
    public void DropItem(Item item)
    {
        //Controls location player will drop item to.
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.5f;

        //Instantiate item that is dropped back into scene. 
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        //Push item away from player to mimic drop. 
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }
}
