using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactMap;
    [SerializeField] private Tile hiddenInteractTile;
    [SerializeField] private Tile interactedTile;

    void Start()
    {
        foreach (var position in interactMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactMap.GetTile(position);

            if(tile!=null && tile.name == "InteractableVis")
            {
                interactMap.SetTile(position, hiddenInteractTile);
            }
        }
    }

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

    public void SetInteracted(Vector3Int position)
    {
        interactMap.SetTile(position, interactedTile);
    }
}
