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

    public float estaminaMax = 100;
    public float estaminaActual = 100;

    private CharacterController characterController;
    private Animator anim;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;

        //Estamina no va
        ControlHood.instancia.ActualizarEstamina(estaminaActual, estaminaMax);

        if(estaminaActual <= 0f)
        {
            estaminaActual = 0f;
            speed = 10f;
        }

        if(estaminaActual >= 100f)
            estaminaActual = 100f;

        if (hor != 0 || ver != 0)
        {
            anim.SetFloat("velocity", 0.7f);
         
            if (Input.GetButtonDown("Jump"))
            {
                estaminaActual -= Time.maximumDeltaTime;
                speed = 15f;
            }
            else
            {
                if(estaminaActual < 100f)
                    estaminaActual += Time.deltaTime;
                speed = 10f;
            }

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
        else
        {
            anim.SetFloat("velocity", 0f);
        }
        //Cambiar
        if (Input.GetButtonUp("Jump"))
        {
            if (estaminaActual < 100f)
                estaminaActual += Time.deltaTime;
            speed = 10f;
        }

        movement.y += gravity * Time.deltaTime;

        characterController.Move(movement);
    }
}
