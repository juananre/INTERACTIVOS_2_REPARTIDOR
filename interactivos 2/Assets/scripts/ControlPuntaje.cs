using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControlPuntaje : MonoBehaviour
{
    [SerializeField]
    private GameObject ui_Puntaje;
    private ControlPickUp1 controlPickup1;
    public static ControlPuntaje Instance { get; private set; }

    public void Start()
    {
        controlPickup1 = ui_Puntaje.GetComponent<ControlPickUp1>();
              
    }
    public void ActualizarPuntaje(int valorPickUp1)
    {
        controlPickup1.ActualizarPuntaje(valorPickUp1);
    }
    public void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this; }
    }
    
}
