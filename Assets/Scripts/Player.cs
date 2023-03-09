using UnityEngine;

//Script which controls player interactions. 

public class Player : MonoBehaviour
{
    public InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    //Function which allows player to drop inventory items. 
    public void DropItem(Item item)
    {
        //Controls location player will drop item to.
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 3f;

        //Instantiate item that is dropped back into scene. 
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        //Push item away from player to mimic drop. 
        droppedItem.rb2d.AddForce(spawnOffset * .01f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i<numToDrop; i++)
        {
            DropItem(item);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "StorageBox")
        {
            UIManager.isCharacterInStorageInteractRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "StorageBox")
        {
            UIManager.isCharacterInStorageInteractRange = false;
           // GameManager.instance.uiManager.CloseStorageUI();
        }
    }
}
