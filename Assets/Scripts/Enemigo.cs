using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemigo : MonoBehaviour
{
    public int vidaMax = 100;
    public int vidaActual = 100;
    public AudioClip sonidoGolpe;
    public bool golpeando;
    public float frecuenciaGolpe;
    public GameObject[] drops;

    private NavMeshAgent EnemigoPruebas;

    public GameObject Player;

    public float EnemyDistanceRun = 4.0f;
    public float EnemyDistanceHit = 3.0f;

    private Animator anim;
    public float frecuenciaGolpeado;
    private float ultimoTiempoGolpeado;
    private AudioSource audioSource;
    private PlayerMovement PlayerMovement;
    private float ultimoTiempoGolpe;

    void Start()
    {
        EnemigoPruebas = GetComponent<NavMeshAgent>();
        PlayerMovement = PlayerMovement.instancia;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.SetFloat("velocity", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.Find("Jugador");
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Seguimiento jugador
        if (distance < EnemyDistanceRun && distance > EnemyDistanceHit)
        {
            anim.SetBool("run", true);
            Vector3 dirToPlayer = transform.position - Player.transform.position;

            GetComponent<NavMeshAgent>().speed = 6;

            Vector3 newPos = transform.position - dirToPlayer;

            EnemigoPruebas.SetDestination(newPos);
        }
        else
        {
            anim.SetBool("run", false);
            anim.SetFloat("velocity", 0f);
        }

        if(distance <= EnemyDistanceHit && PuedeGolpear())
        {
            GetComponent<NavMeshAgent>().speed = 0;
            anim.SetBool("run", false);
            Golpear();
        }
        else
        {
            anim.SetBool("run", true);
        }

        //Muerte
        if (vidaActual <= 0)
        {
            Dropear();
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bate") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 45;
        }

        if (collision.CompareTag("Maletin") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 35;
        }

        if (collision.CompareTag("Pu�o") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 20;
        }
    }

    public void Dropear()
    {
        int drop = Random.Range(0, drops.Length);
        GameObject obj = drops[drop];
        Instantiate(obj, new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), transform.rotation);
    }

    public void Golpear()
    {
        ultimoTiempoGolpe = Time.time;
        anim.SetTrigger("golpe");
        golpeando = true;
        Invoke("Sonido", 0.5f);
    }

    public bool PuedeGolpear()
    {
        if (Time.time - ultimoTiempoGolpe >= frecuenciaGolpe)
            return true;
        return false;
    }

    public void Sonido()
    {
        audioSource.PlayOneShot(sonidoGolpe);
        golpeando = false;
    }

    public bool PuedeSerGolpeado()
    {
        if (Time.time - ultimoTiempoGolpeado >= frecuenciaGolpeado)
            return true;
        return false;
    }
}
