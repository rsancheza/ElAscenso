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
    private Animator anim;
    private bool corriendo;
    private ControlHood hood;

    public static PlayerMovement instancia;

    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        hood = ControlHood.instancia;
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;
        
        if (hood.estaminaActual > 100)
            hood.estaminaActual = 100;

        //Quitar vida con el bot�n A del mando (Para probar la barra de vida sin tener hecho el sistema de pelea)
        if (Input.GetButtonDown("Fire1"))
            Player.instancia.vidaActual -= 10;

        if (hor != 0 || ver != 0)
        {
            anim.SetFloat("velocity", 0.7f);

            if (hood.estaminaActual > 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    corriendo = true;
                }
            }

            if (corriendo)
            {
                hood.ActualizarEstamina(0.01f);
                anim.SetBool("run", true);
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

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir * Time.deltaTime), 0.5f);
        }
        else
        {
            anim.SetFloat("velocity", 0f);
        }

        if (Input.GetButtonUp("Jump"))
        {
            corriendo = false;
            anim.SetBool("run", false);
            speed = 10f;
        }

        movement.y += gravity * Time.deltaTime;

        characterController.Move(movement);
    }
}
