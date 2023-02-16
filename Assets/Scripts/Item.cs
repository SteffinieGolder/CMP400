using UnityEngine;

//Script which sets up in game items. Requires a rigid body 2D. 

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    //The item's scriptable object data. 
    public ItemData data;

    [HideInInspector] public Rigidbody2D rb2d;

    private void Awake()
    { 
        //Initialise rigid body. 
        rb2d = GetComponent<Rigidbody2D>();
    }
}
