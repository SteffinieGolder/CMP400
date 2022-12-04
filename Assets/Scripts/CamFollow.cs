using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform followTransform;
    public Vector3 offset;

    [Range(1, 10)]
    public float smoothFactor;

    public Vector2 minPos;
    public Vector2 maxPos;

    Transform currentTarget;
    int currentRoom;

    private void Start()
    {
        currentTarget = followTransform;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPos = followTransform.position + offset;
        targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }

    public void SetCurrentRoom(int ID)
    {
        currentRoom = ID;
    }

    public int GetCurrentRoom()
    {
        return currentRoom;
    }
}
