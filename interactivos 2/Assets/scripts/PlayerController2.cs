using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO.Ports;
using System;

public class PlayerController2 : MonoBehaviour
{
    public byte run;
    public float cambio = 0f;
    public float speed = 0f;
    private Rigidbody rb;
    private bool acelerador = false;
    private SerialPort _serialPort;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        openSerialPort();


    }

    private void openSerialPort()
    {
        try
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 115200;
            _serialPort.DtrEnable = true;
            _serialPort.NewLine = "\n";
            _serialPort.Open();
            Debug.Log("Open Serial Port");

        }
        catch(Exception e) { 
            Debug.Log(e);

       }

    }

    void Update()
    {

        transform.Translate(Vector3.down * Time.deltaTime * speed);

        
        if (Input.GetKeyDown(KeyCode.C)) { run = 1; }
        if (Input.GetKeyDown(KeyCode.V)) { run = 0; }

        if (_serialPort.BytesToRead > 0)
        {
            string response = _serialPort.ReadLine();
            if (response == "accPressed")
            {
                acelerador = true;
                Debug.Log("accPressed");
            }
            else if (response == "accReleased")
            {
                Debug.Log("accReleased");
                acelerador = false;
            }
        }

        if ((speed < cambio)) { MasSpeed(); } else { MenosSpeed(); }
        if (run == 0) MenosSpeed();
        if (speed <= 0) speed = 0;
        if (cambio <= 0) cambio = 0;
        if (cambio >= 12) cambio = 12;
        if (speed - cambio <= -6) { MenosSpeed(); }


    }
    void MasSpeed()
    {
        // if (run == 1) { if ((Input.GetKey(KeyCode.Space))) { speed += 0.02f; } else { MenosSpeed(); } }
        if (run == 1) { if (true == acelerador) { speed += 0.02f; } else { MenosSpeed(); } }
    }

    void MenosSpeed()
    {
        speed -= 0.02f;
    }
    public void sube()
    {
        Debug.Log("si");
        cambio += 2f;
    }
    public void baja()
    {
        Debug.Log("si");
        cambio -= 2f;
    }
}