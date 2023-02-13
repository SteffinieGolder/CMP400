using UnityEngine;

//Script which makes camera follow player and sets camera boundaries. 
public class CamFollow : MonoBehaviour
{
    //Transform camera should follow and offset.
    public Transform followTransform;
    public Vector3 offset;

    //Variable used for smoothing movement of camera.
    [Range(1, 10)]
    public float smoothFactor;
    //Min and max positions camera can go to (for boundaries). 
    public Vector2 minPos;
    public Vector2 maxPos;

    //Current camera target and the current room camera is in. 
    Transform currentTarget;
    int currentRoom;

    private void Start()
    {
        //Initialise current camera target to the follow transform. 
        currentTarget = followTransform;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    //Function which makes camera follow target. 
    private void Follow()
    {
        //Target position for camera set to the follow transform position with offset applied. 
        Vector3 targetPos = followTransform.position + offset;
        //Clamp position between the boundary points (so camera can't go out of level bounds when following).
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
        
        //For smooth camera movement, lerp between camera's current position and its target. 
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.fixedDeltaTime);
        //Set new camera position. 
        transform.position = smoothPos;
    }

    //Function which sets the current room camera is in. 
    public void SetCurrentRoom(int ID)
    {
        currentRoom = ID;
    }

    //Function which gets the current room camera is in. 
    public int GetCurrentRoom()
    {
        return currentRoom;
    }
}
