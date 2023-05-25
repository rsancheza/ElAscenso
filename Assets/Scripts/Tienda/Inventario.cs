using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventario : MonoBehaviour
{

    [System.Serializable]
    public struct ObjetoInvId
    {
        public int id;
        public int cantidad;

        public ObjetoInvId(int id, int cantidad)
        {
            this.id = id;
            this.cantidad = cantidad;
        }
    }

    [SerializeField]
    DataBase data;

    [Header("Variables del Drag and Drop")]
    public GraphicRaycaster graphRay;
    private PointerEventData pointerData;
    private List<RaycastResult> raycastResults;
    public static Transform canvas;
    public GameObject objetoSeleccionado;
    public Transform exParent;

    [Header("Prefs e items")]
    public static GameObject Descripcion;
    public CartelEliminacion CE;
    public int OSC;
    public int OSID;

    public Transform Contenido;
    public Item item;
    public List<ObjetoInvId> inventario = new List<ObjetoInvId> ();

    void Start()
    {
        InventoryUpdate();

        pointerData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        Descripcion = GameObject.Find("Descripcion");

        CE.gameObject.SetActive(false);

        canvas = transform.parent.transform;
    }

    
    void Update()
    {
        Arrastrar();
    }

    void Arrastrar()
    {
        if (Input.GetMouseButtonDown(1))
        {
            pointerData.position = Input.mousePosition;
            graphRay.Raycast(pointerData, raycastResults);
            if(raycastResults.Count > 0)
            {
                if (raycastResults[0].gameObject.GetComponent<Item>())
                {
                    objetoSeleccionado = raycastResults[0].gameObject;

                    OSC = objetoSeleccionado.GetComponent<Item>().cantidad;
                    OSID = objetoSeleccionado.GetComponent<Item>().ID;

                    exParent = objetoSeleccionado.transform.parent;
                    exParent.GetComponent<Image>().fillCenter = false;
                    objetoSeleccionado.transform.SetParent(canvas);
                }
            }
        }

        if(objetoSeleccionado != null)
        {
            objetoSeleccionado.GetComponent<RectTransform>().localPosition = CanvasScreen(Input.mousePosition);
        }

        if(objetoSeleccionado != null)
        {
            if (Input.GetMouseButtonUp(1))
            {
                pointerData.position = Input.mousePosition;
                raycastResults.Clear();
                graphRay.Raycast(pointerData, raycastResults);

                objetoSeleccionado.transform.SetParent(exParent);

                if(raycastResults.Count > 0)
                {
                    foreach(var resultado in raycastResults)
                    {
                        if (resultado.gameObject == objetoSeleccionado) continue;
                        if (resultado.gameObject.CompareTag("Slot"))
                        {
                            if (resultado.gameObject.GetComponentInChildren<Item>() == null)
                            {
                                objetoSeleccionado.transform.SetParent(resultado.gameObject.transform);
                                Debug.Log("Slot Libre");
                                //objetoSeleccionado.transform.localPosition = Vector2.zero;
                                //exParent = objetoSeleccionado.transform.parent.transform;
                            }
                        }
                        if (resultado.gameObject.CompareTag("Item"))
                        {
                            if (resultado.gameObject.GetComponentInChildren<Item>().ID == objetoSeleccionado.GetComponent<Item>().ID)
                            {
                                Debug.Log("Tienen el mismo ID");
                                resultado.gameObject.GetComponentInChildren<Item>().cantidad += objetoSeleccionado.GetComponent<Item>().cantidad;
                                //Destroy(objetoSeleccionado.gameObject);
                            }
                            else
                            {
                                Debug.Log("Distinto ID");
                                //objetoSeleccionado.transform.SetParent(resultado.gameObject.transform.parent);
                                objetoSeleccionado.transform.SetParent(exParent);
                                resultado.gameObject.transform.SetParent(exParent);
                                resultado.gameObject.transform.localPosition = Vector3.zero;
                                //objetoSeleccionado.transform.localPosition = Vector2.zero;
                            }
                        }
                        if (resultado.gameObject.CompareTag("Eliminar"))
                        {
                            if(objetoSeleccionado.gameObject.GetComponent<Item>().cantidad >= 2)
                            {
                                CE.gameObject.SetActive(true);
                            }
                            else
                            {
                                CE.gameObject.SetActive(false);
                                EliminarItem(objetoSeleccionado.gameObject.GetComponent<Item>().ID, objetoSeleccionado.gameObject.GetComponent<Item>().cantidad);
                            }
                        }
                    }
                }
                objetoSeleccionado.transform.localPosition = Vector3.zero;
                objetoSeleccionado = null;
            }
        }
        raycastResults.Clear();
    }

    public Vector2 CanvasScreen(Vector2 screenPos)
    {
        Vector2 viewPoint = Camera.main.ScreenToViewportPoint(screenPos);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        return (new Vector2(viewPoint.x * canvasSize.x, viewPoint.y * canvasSize.y) - (canvasSize/2));
    }

    public void AgregarItem(int id, int cantidad)
    {
        for (int i =0; i< inventario.Count; i++)
        {
            if(inventario[i].id == id && data.baseDatos[id].acumulable)
            {
                inventario[i] = new ObjetoInvId(inventario[i].id, inventario[i].cantidad + cantidad);
                InventoryUpdate();
                return;
            }

            if (!data.baseDatos[id].acumulable)
                inventario.Add(new ObjetoInvId(id, 1));
            else
                inventario.Add(new ObjetoInvId(id, cantidad));

            InventoryUpdate();
        }
    }
    
    public void EliminarItem(int id, int cantidad)
    {
        for (int i = 0; i<inventario.Count; i++)
        {
            if (inventario[i].id == id)
            {
                inventario[i] = new ObjetoInvId(inventario[i].id, inventario[i].cantidad - cantidad);
                
                if (inventario[i].cantidad <= 0)
                {
                    inventario.Remove(inventario[i]);
                    InventoryUpdate();
                    break;
                }
            }

            InventoryUpdate();
        }
    }

    List<Item> pool = new List<Item>();

    public void InventoryUpdate()
    {
        for(int i = 0; i < pool.Count; i++)
        {
            if (i < inventario.Count)
            {
                ObjetoInvId o = inventario[i];
                pool[i].ID = o.id;
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
                pool[i].cantidad = o.cantidad;
                pool[i].Boton.onClick.RemoveAllListeners();
                pool[i].Boton.onClick.AddListener(() => gameObject.SendMessage(data.baseDatos[o.id].Void, SendMessageOptions.DontRequireReceiver));
                pool[i].gameObject.SetActive(true);
            }
            else
            {
                pool[i].gameObject.SetActive(false);
                pool[i]._descripcion.SetActive(false);
                pool[i].gameObject.transform.parent.GetComponent<Image>().fillCenter = false;
            }
        }

        if(inventario.Count > pool.Count)
        {
            for(int i = pool.Count; i< inventario.Count; i++)
            {
                Item it = Instantiate(item, Contenido.GetChild(i));
                pool.Add(it);

                if(Contenido.GetChild(0).childCount >= 2)
                {
                    for(int s = 0; s < Contenido.childCount; s++)
                    {
                        if(Contenido.GetChild(s).childCount == 0)
                        {
                            it.transform.SetParent(Contenido.GetChild(s));
                            break;
                        }
                    }
                }
                it.transform.position = Vector3.zero;
                it.transform.localScale = Vector3.one;

                ObjetoInvId o = inventario[i];
                pool[i].ID = o.id;
                pool[i].GetComponent<Image>().sprite = data.baseDatos[o.id].icono;
                pool[i].GetComponent<RectTransform>().localPosition = Vector3.zero;
                pool[i].cantidad = o.cantidad;
                pool[i].Boton.onClick.RemoveAllListeners();
                pool[i].Boton.onClick.AddListener(() => gameObject.SendMessage(data.baseDatos[o.id].Void, SendMessageOptions.DontRequireReceiver));
                pool[i].gameObject.SetActive(true);
            }

        }
    }
}
