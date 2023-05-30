using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    public int vidaMax = 100;
    public int vidaActual = 100;

    private NavMeshAgent EnemigoPruebas;

    public GameObject Player;

    public float EnemyDistanceRun = 4.0f;

    public float frecuenciaGolpeado;
    private float ultimoTiempoGolpeado;

    void Start()
    {
        EnemigoPruebas = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Seguimiento jugador
        if (distance < EnemyDistanceRun)
        {
            Vector3 dirToPlayer = transform.position - Player.transform.position;

            Vector3 newPos = transform.position - dirToPlayer;

            EnemigoPruebas.SetDestination(newPos);
        }

        if (vidaActual <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bate") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 35;
        }

        if (collision.CompareTag("Maletin") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 20;
        }

        if (collision.CompareTag("Puño") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            vidaActual -= 10;
        }
    }

    public bool PuedeSerGolpeado()
    {
        if (Time.time - ultimoTiempoGolpeado >= frecuenciaGolpeado)
            return true;
        return false;
    }
}
