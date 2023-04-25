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

    [Header("VentanaPausa")]
    public GameObject ventanaPausa;


    /*[Header("VentanaFinJuego")]
    public GameObject ventanaFinJuego;
    public TextMeshProUGUI resultadoTexto;*/

    public static ControlHood instancia;

    private void Awake()
    {
        instancia = this;
    }

    public void ActualizarVida(int vidaActual, int vidaMax)
    {
        barraVidas.fillAmount = (float)vidaActual / (float)vidaMax;
    }

    public void ActualizarEstamina(float estaminaActual, float estaminaMax)
    {
        barraVidas.fillAmount = estaminaActual / estaminaMax;
    }
}
