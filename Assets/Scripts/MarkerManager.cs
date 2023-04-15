using UnityEngine;
using UnityEngine.Tilemaps;

//Script which controls the marker which appears on screen where the user points their mouse.
public class MarkerManager : MonoBehaviour
{
    //Marker tile.
    [SerializeField] TileBase tile;

    Vector3Int oldCellPosition;
    Tilemap map;

    private void Start()
    {
        map = this.GetComponent<Tilemap>();
    }


    private void Update()
    {
        //Current mouse position on the tile map (environment).
        Vector3Int currentPos = GameManager.instance.tileManager.GetGridPosition(Input.mousePosition, true, TileManager.tilemapOptions.BACKGROUND);

        //Sets the tile to the marker tile if the current position changes.
        if (currentPos != oldCellPosition)
        {
            map.SetTile(oldCellPosition, null);
            map.SetTile(currentPos, tile);
            oldCellPosition = currentPos;
        }
    }
}
