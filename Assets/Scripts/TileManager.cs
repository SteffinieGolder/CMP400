using UnityEngine;
using UnityEngine.Tilemaps;

//Script which controls the tile map and ground tiles in the scene. 

public class TileManager : MonoBehaviour
{
    //Interactable tilemap contains the tiles which can be altered by the player. 
    [SerializeField] private Tilemap interactMap;
    //Tile used to identify the interactable tiles. 
    [SerializeField] private Tile hiddenInteractTile;
    //Tile to switch to once player has interacted. 
    [SerializeField] private Tile interactedTile;

    void Start()
    {
        //Check each tile in the tilemap...
        foreach (var position in interactMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactMap.GetTile(position);

            //If the tile is interactable
            if(tile!=null && tile.name == "InteractableVis")
            {
                //Set it to hidden (so player can't see 'interactable' identifier on tile. 
                interactMap.SetTile(position, hiddenInteractTile);
            }
        }
    }

    //Function which checks if tile is interactable. Returns true if players current position is on interactable tile. 
   public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactMap.GetTile(position);

        if(tile!= null)
        {
            if(tile.name == "Interactable")
            {
                return true;
            }
        }

        return false;
    }

    //Function which changes tile once player has successfully interacted. 
    public void SetInteracted(Vector3Int position)
    {
        interactMap.SetTile(position, interactedTile);
    }
}
