using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

//Script which controls the tile map and ground tiles in the scene. 

public class TileManager : MonoBehaviour
{
    [SerializeField] List<Tilemap> tilemaps;
    [SerializeField] List<TileData> tileDataList;
    [SerializeField] Tile seedTile;

    Dictionary<TileBase, TileData> dataFromTileDict;

    public enum tilemapOptions
    {
        BACKGROUND = 0,
        GROUND = 1
    }

    private void Start()
    {
        dataFromTileDict = new Dictionary<TileBase, TileData>();

        foreach(TileData tileData in tileDataList)
        {
            foreach(TileBase tile in tileData.tiles)
            {
                dataFromTileDict.Add(tile, tileData);
            }
        }  
    }

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

    public Vector3 GetWorldPosition(Vector3Int gridPos, tilemapOptions choice)
    {
        Vector3 worldPos = tilemaps[((int)choice)].CellToWorld(gridPos);
        return worldPos;
    }

    public TileBase GetTileBase(Vector3Int gridPosition, tilemapOptions choice)
    {
        TileBase tile = tilemaps[((int)choice)].GetTile(gridPosition);
        return tile;
    }

    public TileData GetTileData(TileBase tileBase)
    {
        if(dataFromTileDict.ContainsKey(tileBase))
        {
            return dataFromTileDict[tileBase];
        }
        return null;
    }

    public void ChangeTile(Vector3Int position, Tile newTile, tilemapOptions choice)
    {
        tilemaps[((int)choice)].SetTile(position, newTile);

    }

    public void ChangeAllPatchTiles(List<Vector3Int> positions, tilemapOptions option, Tile tileToChangeTo)
    {
        foreach(Vector3Int pos in positions)
        {
            ChangeTile(pos, tileToChangeTo, option);
        }
    }

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