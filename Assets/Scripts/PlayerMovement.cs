using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed = 5f;
    public float rotateSpeed = 100f;

    private float moveInput;
    private float rotateInput;
 

    // Start is called before the first frame update
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
        rotateInput = Input.GetAxisRaw("Horizontal") * rotateSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        transform.Translate(0f, 0f, moveInput);
        transform.Rotate(0f, rotateInput, 0f);
    }
}
