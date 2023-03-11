using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public Sprite choppedSprite;
    public Item choppedObjSpawn;
    public GameObject stumpObj;
    public int amountToSpawn = 3;

    public void ChopTree()
    {
        //Controls location tree will drop item to.
        //Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y - 2f);
        //Vector2 spawnLocation = new Vector2(GameManager.instance.characterManager.activePlayer.transform.position.x - 2f, GameManager.instance.characterManager.activePlayer.transform.position.y);

        //Controls location player will drop item to.
        Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y - 2f);
        Vector2 spawnOffset = Random.insideUnitCircle * 1f;

        //Instantiate item that is dropped back into scene. 
        for (int i = 0; i < amountToSpawn; i++)
        {
            Item log = Instantiate(choppedObjSpawn, GameManager.instance.characterManager.activePlayer.gameObject.GetComponent<CharMovement>().GetItemSpawnPos(), Quaternion.identity);

            //Push item away from player to mimic drop. 
            //log.rb2d.AddForce(spawnOffset * .01f, ForceMode2D.Impulse);
        }

        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y - 1f);
        Instantiate(stumpObj, spawnPos, transform.rotation);
        Destroy(this.gameObject);
    }
}
