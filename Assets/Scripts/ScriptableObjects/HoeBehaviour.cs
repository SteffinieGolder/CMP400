using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tool Behaviour/Hoe")]

public class HoeBehaviour : ToolBehaviour
{
    public Tile PlowTile;
    TileManager manager;
    TileData tileData;
    Vector3Int gridPos;

    public override bool CheckUseConditions(Vector2 position)
    {
        manager = GameManager.instance.tileManager;
        gridPos = manager.GetGridPosition(position, true);

        /////////THIS IS JANKY AS HELL? AND NEEDS FIXED BUT JUST FOR TESTING RN
        /////SOME TILES ARE ON THE OTHER TILEMAP - EITHER COMBINE INTO ONE OR FIGURE OUT HOW THE LAYERING WILL WORK. 

        TileBase tile = manager.GetTileBase(gridPos);

        if (tile)
        {
            tileData = manager.GetTileData(tile);

            if (tileData)
            {
                if (tileData.isPlowable)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override bool PerformBehaviour()
    {
        //Set char animation to holding the tool
        //Whenever player clicks to use, animate action
        //Set tile to plowed tile. 

        manager.ChangeTile(gridPos, PlowTile);
        return true;
    }
}
