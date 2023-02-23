using UnityEngine;
using UnityEngine.Tilemaps;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] TileBase tile;

    Vector3Int oldCellPosition;
    Tilemap map;

    private void Start()
    {
        map = this.GetComponent<Tilemap>();
    }


    private void Update()
    {
        Vector3Int currentPos = GameManager.instance.tileManager.GetGridPosition(Input.mousePosition, true, TileManager.tilemapOptions.BACKGROUND);

        if (currentPos != oldCellPosition)
        {
            map.SetTile(oldCellPosition, null);
            map.SetTile(currentPos, tile);
            oldCellPosition = currentPos;
        }
    }
}
