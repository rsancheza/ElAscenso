using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int vidaMax = 100;
    public int vidaActual = 100;

    public static Player instancia;

    private void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        ControlHood.instancia.ActualizarVida(vidaActual, vidaMax);
        if(vidaActual <= 0)
            ControlHood.instancia.EstablecerVentanaFinJuego(false);
    }
}
/*
 Ataque
 Monedas
Animacion Puñetazo
 */