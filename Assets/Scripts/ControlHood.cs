using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlHood : MonoBehaviour
{
    [Header("Hood")]
    public TextMeshProUGUI puntuacionTexto;
    public Image barraVidas;
    public Image barraEstamina;

    public float estaminaMax = 100;
    public float estaminaActual;

    [Header("VentanaPausa")]
    public GameObject ventanaPausa;


    [Header("VentanaFinJuego")]
    public GameObject ventanaFinJuego;
    public TextMeshProUGUI resultadoTexto;

    public static ControlHood instancia;

    private Coroutine regeneracion, gastar;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        estaminaActual = estaminaMax;
        barraEstamina.fillAmount = estaminaMax;
    }

    public void ActualizarPuntuacion(int puntos)
    {
        puntuacionTexto.text = puntos.ToString("0000");
    }

    //Vida
    public void ActualizarVida(int vidaActual, int vidaMax)
    {
        barraVidas.fillAmount = (float)vidaActual / (float)vidaMax;
    }

    //Estamina
    public void ActualizarEstamina(float cantidad)
    {
        if(estaminaActual - cantidad >= 0)
        {
            estaminaActual -= cantidad;
            barraEstamina.fillAmount = (float)estaminaActual / (float)estaminaMax;
            PlayerMovement.instancia.speed = 15f;

            if (regeneracion != null)
                StopCoroutine(regeneracion);

            regeneracion = StartCoroutine(RegenerarEstamina());
        }
        else
        {
            PlayerMovement.instancia.speed = 10f;
            Debug.Log("No tienes estamina");
        }
    }

    IEnumerator RegenerarEstamina()
    {
        yield return new WaitForSeconds(0.2f);

        while (estaminaActual < estaminaMax)
        {
            estaminaActual += estaminaMax / 200;
            barraEstamina.fillAmount = (float)estaminaActual / (float)estaminaMax;
            yield return new WaitForSeconds(0.05f);
        }
        regeneracion = null;
    }

    //Fin del Juego
    public void EstablecerVentanaFinJuego(bool ganado)
    {
        ventanaFinJuego.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        resultadoTexto.text = ganado ? "HAS GANADO" : "HAS PERDIDO";
        resultadoTexto.color = ganado ? Color.green : Color.red;
        Time.timeScale = 0f;
        Destroy(PlayerMovement.instancia);
    }
}
