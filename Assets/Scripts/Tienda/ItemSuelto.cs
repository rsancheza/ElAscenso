using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSuelto : MonoBehaviour
{
    public int cantidad;
    public int ID;
    public Inventario Inv;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inv.AgregarItem(ID, cantidad);
        }
    }
}
