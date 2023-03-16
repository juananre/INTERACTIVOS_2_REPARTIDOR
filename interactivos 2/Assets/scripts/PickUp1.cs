    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


public class PickUp1 : MonoBehaviour
{

    [SerializeField]
    private int valorPickUp1 = 1;
    public GameObject carrito5;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        ControlPuntaje.Instance.ActualizarPuntaje(valorPickUp1);
    }
}


