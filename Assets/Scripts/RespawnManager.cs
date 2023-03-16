using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] GameObject objectToRespawn;
    [SerializeField] GameObject objectToRemove;
    [SerializeField] List<Vector3> objectSpawnPositions;
    [SerializeField] Tile tileToRespawn;
    [SerializeField] TileManager.tilemapOptions option;
    [SerializeField] List<Vector2> worldLocations;

    public void RespawnObject()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stump"))
        {
            Destroy(obj);
        }
            foreach (Vector3 pos in objectSpawnPositions)
        {
            Instantiate(objectToRespawn, pos, Quaternion.identity);
        }
    }

    public void RespawnTiles()
    {
        List<Vector3Int> allGridPositions = GameManager.instance.tileManager.GetAllPatchGridPositions(worldLocations, option);

        GameManager.instance.tileManager.ChangeAllPatchTiles(allGridPositions, option, tileToRespawn);
    }
}
