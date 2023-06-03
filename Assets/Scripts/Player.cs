using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int vidaMax = 100;
    public int vidaActual = 100;
    public GameObject bate, maletin;
    public int maxEstus = 5;

    public int puntos;

    public static Player instancia;

    public float frecuenciaGolpeado;
    private float ultimoTiempoGolpeado;

    private void Awake()
    {
        instancia = this;
    }

    void Update()
    {
        ControlHood.instancia.ActualizarVida(vidaActual, vidaMax);

        if(vidaActual <= 0)
            ControlHood.instancia.EstablecerVentanaFinJuego(false);

        ControlHood.instancia.ActualizarPuntuacion(puntos);
        ControlHood.instancia.ActualizarCuraciones(maxEstus);

        if (vidaActual > 100)
            vidaActual = 100;

        if(puntos < 0)
            puntos = 0;

        if (Input.GetButtonDown("Fire2") && maxEstus > 0 && vidaActual < vidaMax && vidaActual > 0)
        {
            Debug.Log("Curando");
            Curar();
        }
    }

    public void Curar()
    {
        vidaActual += 20;
        maxEstus--;
    }

    public void SumarPuntos(int a)
    {
        puntos += a;
    }

    public void RestarPuntos(int a)
    {
        puntos -= a;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("ArmaEnemigo1") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            RestarPuntos(1);
            vidaActual -= 3;
        }

        if (collision.CompareTag("ArmaEnemigo2") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            RestarPuntos(2);
            vidaActual -= 5;
        }

        if (collision.CompareTag("ArmaEnemigo3") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            RestarPuntos(5);
            vidaActual -= 10;
        }

        if (collision.CompareTag("ArmaMiniBoss") && PuedeSerGolpeado())
        {
            ultimoTiempoGolpeado = Time.time;
            RestarPuntos(10);
            vidaActual -= 15;
        }
        
        if (collision.CompareTag("BateRecoger"))
        {
            bate.SetActive(true);
            maletin.SetActive(false);
            Destroy(collision.gameObject);     
        }

        if (collision.CompareTag("MaletinRecoger"))
        {
            maletin.SetActive(true);
            bate.SetActive(false);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Estus"))
        {
            if (maxEstus >= 0)
                maxEstus++;
            Destroy(collision.gameObject);
        }
    }

    public bool PuedeSerGolpeado()
    {
        if (Time.time - ultimoTiempoGolpeado >= frecuenciaGolpeado)
            return true;
        return false;
    }
}