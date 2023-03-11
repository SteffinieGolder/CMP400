using UnityEngine;

//Script which controls player character movement and animation. 
public class CharMovement : MonoBehaviour
{
    //Movement speed variable. 
    public float moveSpeed;
    //Animator variable.
    public Animator animator;
    //Direction of player character. 
    private Vector3 direction;

    public float itemSpawnOffset = 1f;
    Vector3 lastDirFacing;

    void Update()
    {
        //Get horizontal and vertical input axis for movement calculations. 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Set direction to normalised vector to prevent faster movement in diagonal direction. 
        direction = new Vector3(horizontal, vertical,0).normalized;

        if(direction!=new Vector3(0,0,0))
        {
            lastDirFacing = direction;
        }
        
        //Pass direction to animation function. 
        AnimateMovement(direction);
    }

    private void FixedUpdate()
    {
        //Move player character.
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    //Function which animates the player character based on movement input/calculations. 
    void AnimateMovement(Vector3 movedir)
    {
        if(animator!=null)
        {
            //If player character is moving update animator variables based on input.
            if(movedir.magnitude>0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", movedir.x);
                animator.SetFloat("vertical", movedir.y);
            }

            //Otherwise, tell animator player has stopped moving. 
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    public Vector3 GetItemSpawnPos()
    {
        if(lastDirFacing.x >= 1)
        {
            return new Vector3(transform.position.x - itemSpawnOffset, transform.position.y, 0);
        }

        if (lastDirFacing.x < 0)
        {
            return new Vector3(transform.position.x + itemSpawnOffset, transform.position.y, 0);
        }

        if (lastDirFacing.y >= 1)
        {
            return new Vector3(transform.position.x, transform.position.y - itemSpawnOffset, 0);
        }

        if (lastDirFacing.y < 0)
        {
            return new Vector3(transform.position.x, transform.position.y + itemSpawnOffset, 0);
        }
        return new Vector3(0, 0, 0);
    }
}
