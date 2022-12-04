using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public float moveSpeed;
    //Should I get this from gameobject?
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Normalising this vector prevents faster movement in diagonal direction. 
        Vector3 direction = new Vector3(horizontal, vertical,0).normalized;

        AnimateMovement(direction);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void AnimateMovement(Vector3 movedir)
    {
        if(animator!=null)
        {
            if(movedir.magnitude>0)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("horizontal", movedir.x);
                animator.SetFloat("vertical", movedir.y);
            }

            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}
