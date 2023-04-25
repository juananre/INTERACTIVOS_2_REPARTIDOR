using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class ControlTiempo : MonoBehaviour
{
    [SerializeField] TMP_Text txt_contador_Tiempo;
    [SerializeField] int cambio_estado = 0;
    [SerializeField] int min, seg;

    private float restante;

    void Start()
    {

    }
    void TerminarTemporizador()
    {
        SceneManager.LoadScene(2);

    }
    void Update()
    {

        restante -= Time.deltaTime;
        int tempmin = Mathf.FloorToInt(restante / 60);
        int tempseg = Mathf.FloorToInt(restante % 60);
        txt_contador_Tiempo.text = string.Format("{00:00}:{01:00}", tempmin, tempseg);

        if (Input.GetKeyDown(KeyCode.Space) || restante <= 1)
        {
            TerminarTemporizador();
        }

    }
    public void Awake()
    {
        restante = (min * 60) + seg;
    }
}