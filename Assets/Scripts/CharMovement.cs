using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{
    public float moveSpeed;
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Normalising this vector prevents faster movement in diagonal direction. 
        Vector3 direction = new Vector3(horizontal, vertical).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
