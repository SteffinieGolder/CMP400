using UnityEngine;

//Game manager script which controls various game elements. 
public class GameManager : MonoBehaviour
{
    //Static instance so only one game manager can exist. 
    public static GameManager instance;
    //Item manager script which controls in game items. 
    public ItemManager itemManager;
    //Tile manager script which controls ground environment tiles in the level. 
    public TileManager tileManager;
    //UI manager script which controls the UI in the game.
    public UIManager uiManager;
    //Script which manages the day and night cycle.
    public DayAndNightManager dayAndNightManager;
    //Script which manages characters.
    public CharacterManager characterManager;
    //Script which manages tasks.
    public TaskManager taskManager;
    //Script which manages item respawning. 
    public RespawnManager respawnManager;

    private void Awake()
    {
        //Destory instance if another already exists. 
        if(instance !=null && instance !=this)
        {
            Destroy(this.gameObject);
        }

        //Set game manager instance to this if one doesn't exist already. 
        else
        {
            instance = this;
        }

        //Ensure this object persists. 
        DontDestroyOnLoad(this.gameObject);

        //Set manager variables.
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UIManager>();
        dayAndNightManager = GetComponent<DayAndNightManager>();
        characterManager = GetComponent<CharacterManager>();
        taskManager = GetComponent<TaskManager>();
        respawnManager = GetComponent<RespawnManager>();
    }
}
