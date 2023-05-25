using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CartelEliminacion : MonoBehaviour
{
    private Inventario Inv;
    public Slider slider;
    public TextMeshProUGUI CantidadText;

    void Start()
    {
        Inv = GameObject.Find("Tienda").GetComponent<Inventario>();        
    }

    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            slider.maxValue = Inv.OSC;
            CantidadText.text = slider.value.ToString();
        }
    }

    public void Aceptar()
    {
        Inv.EliminarItem(Inv.OSID, Mathf.RoundToInt(slider.value));
        Debug.Log("Se aceptó eliminar: " + Mathf.RoundToInt(slider.value) + " items con ID: " + Inv.OSID);
        slider.value = 1;
        this.gameObject.SetActive(false);
    }

    public void Cancelar()
    {
        slider.value = 1;
        this.gameObject.SetActive(false);
    }
}
