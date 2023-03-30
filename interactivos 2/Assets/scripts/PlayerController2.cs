using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController2 : MonoBehaviour
{
    public byte run;
    public float cambio = 0f;
    public float speed = 0f;
    private Rigidbody rb;

    [Header("Audios")]
    [SerializeField] AudioSource audio_Arranque;
    [SerializeField] AudioSource audio_neutro;
    [SerializeField] AudioSource audio_movimiento;
    [SerializeField] AudioSource audio_freno;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    void Update()
    {

        transform.Translate(Vector3.down * Time.deltaTime * speed);


        if (Input.GetKeyDown(KeyCode.C)) 
        {
            run = 1;
            encendido();
        }
        if (Input.GetKeyDown(KeyCode.V)) 
        {
            run = 0; 
        }
        if ((speed < cambio)) 
        { 
            MasSpeed(); 
        } 
        else 
        {
            MenosSpeed(); 
        }
        if (run == 0) MenosSpeed();
        if (speed <= 0) speed = 0;
        if (cambio <= 0) cambio = 0;
        if (cambio >= 12) cambio = 12;
        if (speed - cambio <= -6) { MenosSpeed(); }


    }
    void MasSpeed()
    {
        if (run == 1)
        { 
            if (Input.GetKey(KeyCode.Space)) 
            { 
                speed += 0.02f;
                audio_neutro.Stop();
                audio_movimiento.Play();
            }
            else 
            {
                MenosSpeed();
                audio_movimiento.Stop();
                audio_freno.Play();
            }
        }
    }
    void MenosSpeed()
    {
        speed -= 0.02f;
    }
    public void sube()
    {
        cambio += 2f;
    }
    public void baja()
    {
        cambio -= 2f;
    }
    public void encendido()
    {
        audio_Arranque.Play();
        audio_neutro.PlayDelayed(2f);
    }
 
}