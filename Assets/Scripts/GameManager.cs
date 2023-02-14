using UnityEngine;

//Game manager script which controls various game elements. 
public class GameManager : MonoBehaviour
{
    //Static instance so only one game manager can exist. 
    public static GameManager instance;
    //Item manager script which controls in game items. 
    public ItemManager itemManager;
    public TileManager tileManager;

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
        //Set item manager. 
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
    }
}