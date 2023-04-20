using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class ControlPickUp1 : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt_contador_pickup1;
    public static int contador_pickup1;
    

    public void Start()
    {
        
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
        if (contador_pickup1 == 6)
        {
            SceneManager.LoadScene(3);
        }
        txt_contador_pickup1.text = "" + contador_pickup1;
    }
}
