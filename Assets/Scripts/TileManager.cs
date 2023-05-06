using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

//Script which controls the tile map and ground tiles in the scene. 
//Code adapted from this tutorial series by Greg Dev Stuff: https://www.youtube.com/watch?v=ZIEE-2ZdAxU&list=PL0GUZtUkX6t6wXF0U0WAQNVYL68pYUCZv&ab_channel=GregDevStuff 

public class TileManager : MonoBehaviour
{
    //Tilemaps for the game.
    [SerializeField] List<Tilemap> tilemaps;
    //List of tile data objects (defines the type of tile and how it can be used).
    [SerializeField] List<TileData> tileDataList;
    //Carrot seed tile.
    [SerializeField] Tile seedTile;

    //Dictionary which matches a tile to its data.
    Dictionary<TileBase, TileData> dataFromTileDict;

    //Corresponds to index positions in the tilemap list.
    public enum tilemapOptions
    {
        BACKGROUND = 0,
        GROUND = 1
    }

    private void Start()
    {
        //Initialise dictionary of tiles.
        dataFromTileDict = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in tileDataList)
        {
            foreach(TileBase tile in tileData.tiles)
            {
                dataFromTileDict.Add(tile, tileData);
            }
        }  
    }

    //Returns the position in the tilemap which corresponds to a world/mouse position.
    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition, tilemapOptions choice)
    {
        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPos = tilemaps[((int)choice)].WorldToCell(worldPosition);
        return gridPos;
    }

    //Returns the world position of a tile.
    public Vector3 GetWorldPosition(Vector3Int gridPos, tilemapOptions choice)
    {
        Vector3 worldPos = tilemaps[((int)choice)].CellToWorld(gridPos);
        return worldPos;
    }

    //Returns the tile.
    public TileBase GetTileBase(Vector3Int gridPosition, tilemapOptions choice)
    {
        TileBase tile = tilemaps[((int)choice)].GetTile(gridPosition);
        return tile;
    }

    //Returns the tile data.
    public TileData GetTileData(TileBase tileBase)
    {
        if(dataFromTileDict.ContainsKey(tileBase))
        {
            return dataFromTileDict[tileBase];
        }
        return null;
    }

    //Changes a tile.
    public void ChangeTile(Vector3Int position, Tile newTile, tilemapOptions choice)
    {
        tilemaps[((int)choice)].SetTile(position, newTile);

    }

    //Changes a patch of tiles.
    public void ChangeAllPatchTiles(List<Vector3Int> positions, tilemapOptions option, Tile tileToChangeTo)
    {
        foreach(Vector3Int pos in positions)
        {
            ChangeTile(pos, tileToChangeTo, option);
        }
    }

    //Returns a patch of positions in the tilemap.
    public List<Vector3Int> GetAllPatchGridPositions(List<Vector2> positions, tilemapOptions option)
    {
        List<Vector3Int> gridPositions = new List<Vector3Int>();

        foreach(Vector2 pos in positions)
        {
            Vector3Int gridPos = tilemaps[((int)option)].WorldToCell(pos);
            gridPositions.Add(gridPos);
        }

        return gridPositions;
    }
}