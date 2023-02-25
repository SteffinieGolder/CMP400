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
        Vector2 spawnLocation = new Vector2(transform.position.x, transform.position.y - 2f);

        //Instantiate item that is dropped back into scene. 
        for (int i = 0; i < amountToSpawn; i++)
        {
            Item log = Instantiate(choppedObjSpawn, spawnLocation, Quaternion.identity);
        }

        Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y - 1f);
        Instantiate(stumpObj, spawnPos, transform.rotation);
        Destroy(this.gameObject);
    }
}
