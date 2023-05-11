using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp1 : MonoBehaviour
{

    [SerializeField]
    private int valorPickUp1 = 1;
    public GameObject pickup;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ControlPuntaje.Instance.ActualizarPuntaje(valorPickUp1);
            gameObject.SetActive(false);
            
        }
       
    }
}


