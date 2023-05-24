using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CtrlTiempo : MonoBehaviour
{
    [SerializeField] private TMP_Text txt_contador_Tiempo;
    private int contador_Tiempo = 600;
    private int i = 0;
    public static CtrlTiempo Instance { get; private set; }
    [SerializeField] GameObject perdiste;


    void Start()
    {
        Invoke("TerminarTemporizador", 600f);

        InvokeRepeating("ActualizarTiempo", 0f, 1f);
    }

    public void ActualizarTiempo()
    {
        contador_Tiempo -= 1;
        ActualizarValorUI();
    }
    public void SumarTiempo(int valor)
    {
        contador_Tiempo += valor;
        ActualizarValorUI();
    }
    public void ActualizarValorUI()
    {
        txt_contador_Tiempo.text = "" + contador_Tiempo;
    }
    void TerminarTemporizador()
    {
        
        perdiste.SetActive(true);

    }
    public void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this; }
    }
}