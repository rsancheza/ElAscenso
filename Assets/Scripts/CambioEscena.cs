using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public int escena;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainManager.instance.puntuacionTotal += Player.instancia.puntos;
            SceneManager.LoadScene(escena);
        }
    }
}
