using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    public int vidaMax = 100;
    public int vidaActual = 100;
    public AudioClip sonidoGolpe;
    public bool golpeando;
    public float frecuenciaGolpe;

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

    public static Enemigo instancia;

    private void Awake()
    {
        instancia = this;
    }

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
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Seguimiento jugador
        if (distance < EnemyDistanceRun && distance > EnemyDistanceHit)
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;

            GetComponent<NavMeshAgent>().speed = 6;
            anim.SetBool("run", true);

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

        if (vidaActual <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bate") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 35;
        }

        if (collision.CompareTag("Maletin") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 20;
        }

        if (collision.CompareTag("Puño") && PuedeSerGolpeado() && PlayerMovement.golpeando)
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 10;
        }
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
