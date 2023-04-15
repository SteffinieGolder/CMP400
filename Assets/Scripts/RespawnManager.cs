using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Script which respawns tiles and gameobjects when new day starts.

public class RespawnManager : MonoBehaviour
{
    //Objects to spawn and remove.
    [SerializeField] GameObject objectToRespawn;
    [SerializeField] GameObject objectToRemove;
    //Positions.
    [SerializeField] List<Vector3> objectSpawnPositions;
    //Tile to respawn (fishing bubble).
    [SerializeField] Tile tileToRespawn;
    //Which tilemap to use.
    [SerializeField] TileManager.tilemapOptions option;
    //World locations of tiles.
    [SerializeField] List<Vector2> worldLocations;

    //Respawns the tree objects.
    public void RespawnObject()
    {
        //Destroys all stumps and spawns full trees.
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stump"))
        {
            Destroy(obj);
        }
            foreach (Vector3 pos in objectSpawnPositions)
        {
            Instantiate(objectToRespawn, pos, Quaternion.identity);
        }
    }

    //Respawns tiles.
    public void RespawnTiles()
    {
        //Respawns fishing bubble tiles at their locations.
        List<Vector3Int> allGridPositions = GameManager.instance.tileManager.GetAllPatchGridPositions(worldLocations, option);

        GameManager.instance.tileManager.ChangeAllPatchTiles(allGridPositions, option, tileToRespawn);
    }
}
