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


    /*[Header("VentanaFinJuego")]
    public GameObject ventanaFinJuego;
    public TextMeshProUGUI resultadoTexto;*/

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

    public void ActualizarVida(int vidaActual, int vidaMax)
    {
        barraVidas.fillAmount = (float)vidaActual / (float)vidaMax;
    }

    public void ActualizarEstamina(float cantidad)
    {
        if(estaminaActual - cantidad >= 0)
        {
            if (gastar != null)
                StopCoroutine(gastar);

            gastar = StartCoroutine(QuitarEstamina());

            if (regeneracion != null)
                StopCoroutine(regeneracion);

            regeneracion = StartCoroutine(RegenerarEstamina());

            /*estaminaActual -= cantidad;
            barraEstamina.fillAmount = estaminaActual;*/
        }
        else
        {
            Debug.Log("No tienes estamina");
        }
    }

    IEnumerator RegenerarEstamina()
    {
        yield return new WaitForSeconds(1);

        while (estaminaActual < estaminaMax)
        {
            estaminaActual += estaminaMax / 100;
            barraEstamina.fillAmount = estaminaActual;
            yield return new WaitForSeconds(0.1f);
        }
        regeneracion = null;
    }

    IEnumerator QuitarEstamina()
    {
        while (estaminaActual >= 0)
        {
            estaminaActual -= estaminaMax / 100;
            barraEstamina.fillAmount = estaminaActual;
            yield return new WaitForSeconds(0.1f);
        }
        gastar = null;
    }
}
