using UnityEngine;

//Script which spawns a tree stump and logs when user chops down a tree.

public class TreeScript : MonoBehaviour
{
    //Stump sprite.
    public Sprite choppedSprite;
    //Log object.
    public Item choppedObjSpawn;
    //Stump gameobject.
    public GameObject stumpObj;
    //Amount of logs to spawn per tree.
    public int amountToSpawn = 3;

    public void ChopTree()
    {
        //Controls location tree will drop item to.
        Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y - 2f);
        Vector2 spawnOffset = Random.insideUnitCircle * 1f;

        //Instantiate item that is dropped into scene. 
        for (int i = 0; i < amountToSpawn; i++)
        {
            Item log = Instantiate(choppedObjSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), Quaternion.identity);
        }

        //Instantiate the tree stump object.
        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y - 1f);
        Instantiate(stumpObj, spawnPos, transform.rotation);
        //Destory the tree object.
        Destroy(this.gameObject);
    }
}
