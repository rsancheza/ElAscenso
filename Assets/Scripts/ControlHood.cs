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

    public GameObject Tienda;

    public static ControlHood instancia;

    private Coroutine regeneracion;
    private Animator anim;
    private bool juegoPausado;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        estaminaActual = estaminaMax;
        barraEstamina.fillAmount = estaminaMax;
        anim = PlayerMovement.instancia.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            CambiarPausa();
        }
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
            anim.SetBool("run", true);

            if (regeneracion != null)
                StopCoroutine(regeneracion);

            regeneracion = StartCoroutine(RegenerarEstamina());
        }
        else
        {
            PlayerMovement.instancia.speed = 10f;
            anim.SetBool("run", false);
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

    //Pausa
    public void CambiarEstadoVentanaPausa(bool estado)
    {
        Tienda.SetActive(estado);
    }

    public void CambiarPausa()
    {
        juegoPausado = !juegoPausado;
        //Time.timeScale = (juegoPausado ? 0.0f : 1f);
        Cursor.lockState = (juegoPausado) ? CursorLockMode.None : CursorLockMode.Locked;
        CambiarEstadoVentanaPausa(juegoPausado);
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
