using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerArmas : MonoBehaviour
{
    public bool entra = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            entra = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        entra = false;
    }

    private void OnGUI()
    {
        if (entra)
        {
            GUI.Box(new Rect(414, 126, 351, 32), "Presiona Q o X (JoyStick) para agarrar");
        }
    }
}
