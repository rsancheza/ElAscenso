using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform camara;
    public float speed = 10f;
    public float fuerzaSalto;
    public float fallVelocity;
    public float gravity = -9.8f;

    private CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;

        if (hor != 0 || ver != 0)
        {
            Vector3 forward = camara.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = camara.right;
            right.y = 0;
            right.Normalize();

            Vector3 dir = forward * ver + right * hor;
            dir.Normalize();

            movement = dir * speed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.5f);
        }

        /*if (characterController.isGrounded && movement.y < 0)
            movement.y = -2;
         
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            Debug.Log("Salto");
            movement.y = Mathf.Sqrt(fuerzaSalto * -2 * -gravity);
        }*/

        movement.y += gravity * Time.deltaTime;

        characterController.Move(movement);

    }
}
