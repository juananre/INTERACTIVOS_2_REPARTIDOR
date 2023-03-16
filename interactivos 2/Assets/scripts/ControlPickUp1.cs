using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class ControlPickUp1 : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_contador_pickup1;
    public static int contador_pickup1;

    public void Start()
    {
        if (contador_pickup1 == 3)
        {
            Debug.Log("Ha consegido 5 pickups");
        }
    }
    public int CantidadPickUpsRecolectados()
    {

        return contador_pickup1;
    }
    public void ActualizarPuntaje(int valor)
    {
        contador_pickup1 += 1;
        ActualizarValorUI();
    }
    private void ActualizarValorUI()
    {
        txt_contador_pickup1.text = "" + contador_pickup1;
    }
}
