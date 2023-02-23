using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public Sprite choppedSprite;
    public Item choppedObjSpawn;
    public int amountToSpawn = 3;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChopTree()
    {
        spriteRenderer.sprite = choppedSprite;

        //Controls location player will drop item to.
        Vector2 spawnLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 3f;

        //Instantiate item that is dropped back into scene. 
        for (int i = 0; i < amountToSpawn; i++)
        {
            Item log = Instantiate(choppedObjSpawn, spawnLocation + spawnOffset, Quaternion.identity);
            //Push item away from tree to mimic drop. 
            log.rb2d.AddForce(spawnOffset * .01f, ForceMode2D.Impulse);
        }

        Destroy(this.gameObject);
    }
}
