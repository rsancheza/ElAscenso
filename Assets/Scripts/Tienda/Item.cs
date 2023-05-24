using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI CantidadText;
    public int cantidad = 0;
    public int ID;
    public bool acumulable;
    public Button Boton;
    public GameObject _descripcion;
    public TextMeshProUGUI Nombre_;
    public TextMeshProUGUI Dato_;
    public Vector3 offset;
    public DataBase DB;

    void Start()
    {
        acumulable = DB.baseDatos[ID].acumulable;
        Boton = GetComponent<Button>();
        _descripcion = Inventario.Descripcion;
        Nombre_ = _descripcion.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Dato_ = _descripcion.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _descripcion.SetActive(false);
        if (!_descripcion.GetComponent<Image>().enabled)
        {
            _descripcion.GetComponent<Image>().enabled = true;
            Nombre_.enabled = true;
            Dato_.enabled = true;
        }
    }

    void Update()
    {
        if(transform.parent.GetComponent<Image>() != null)
        {
            transform.parent.GetComponent<Image>().fillCenter = true;
        }

        CantidadText.text = cantidad.ToString();

        if(transform.parent == Inventario.canvas)
        {
            _descripcion.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData evventData)
    {
        _descripcion.SetActive(true);
        Nombre_.text = DB.baseDatos[ID].nombre;
        Dato_.text = DB.baseDatos[ID].descripcion;
        _descripcion.transform.position = transform.position + offset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _descripcion.SetActive(false);
    }
}
