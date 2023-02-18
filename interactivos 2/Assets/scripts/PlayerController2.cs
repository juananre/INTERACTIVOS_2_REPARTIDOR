using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController2 : MonoBehaviour
{
    public byte run;
    public float cambio = 0f;
    public float speed = 0f;
    private Rigidbody rb;
   
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
         
         transform.Translate(Vector3.fwd * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.W))
        {
            cambio += 10f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cambio -= 10f;
        }
        if (Input.GetKeyDown(KeyCode.C)) { run =1;}
        if (Input.GetKeyDown(KeyCode.V)) { run = 0; }
        if ((speed < cambio)) { MasSpeed(); } else { MenosSpeed(); }
        if (run == 0) MenosSpeed();
        if (speed <= 0) speed = 0;
        if (cambio <= 0) cambio = 0;
        if (cambio >= 50) cambio = 50;
      
        
    }
    void MasSpeed()
    {   if (run == 1) { speed += 0.02f;}
     
    }
    void MenosSpeed()
    {
         speed -= 0.02f; 
    }

}