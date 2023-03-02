using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;

public class comm : MonoBehaviour
{
    static int incinacion;
    private static comm instance;
    private Thread receiveThread;
    private UdpClient receiveClient;
    private IPEndPoint receiveEndPoint;
    public string ip = "127.0.0.1";
    public int receivePort = 64;
    private bool isInitialized;
    private Queue receiveQueue;
    public GameObject cube;
    //private Material m_Material;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {

        //m_Material = cube.GetComponent<Renderer>().material;
    }

    private void Initialize()
    {
        instance = this;
        receiveEndPoint = new IPEndPoint(IPAddress.Parse(ip), receivePort); //recibe la informacion
        receiveClient = new UdpClient(receivePort);
        receiveQueue = Queue.Synchronized(new Queue()); //puerta
        receiveThread = new Thread(new ThreadStart(ReceiveDataListener)); //Crear nuevo hilo
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;
    }

    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] data = receiveClient.Receive(ref receiveEndPoint);
                string text = Encoding.UTF8.GetString(data);
                zigSimData zigSimdata = zigSimData.CreateFromJSON(text);
                //Debug.Log(zigSimdata.sensordata.gravity.x + "," + zigSimdata.sensordata.gravity.y);
                /*
                double coorX = zigSimdata.sensordata.gyro.x;
                double coorY = zigSimdata.sensordata.gyro.y;
                double coorZ = zigSimdata.sensordata.gyro.z;
                */
                float[] coordenadas = (new float[] { zigSimdata.sensordata.gravity.x, zigSimdata.sensordata.gravity.y, zigSimdata.sensordata.gravity.z });

                //byte[] dataBytes = new byte[12];
                /*
                Array.Copy(BitConverter.GetBytes(zigSimdata.sensordata.gyro.x), 0, dataBytes, 0, 4);
                Array.Copy(BitConverter.GetBytes(zigSimdata.sensordata.gyro.y), 0, dataBytes, 4, 4);
                Array.Copy(BitConverter.GetBytes(zigSimdata.sensordata.gyro.z), 0, dataBytes, 8, 4);
                */

                SerializeMessage(coordenadas);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }

    private void SerializeMessage(float[] menssage)
    {
        try
        {

            receiveQueue.Enqueue((System.Object)menssage);



            /*
             string[] chain = message.Split(' ');
             string key = chain[0];
             float value = 0;
             if (float.TryParse(chain[1], out value))
             {
                 receiveQueue.Enqueue(value);
             }
            */
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void OnDestroy()
    {
        TryKillThread();
    }

    private void OnApplicationQuit()
    {
        TryKillThread();
    }

    private void TryKillThread()
    {
        if (isInitialized)
        {
            receiveThread.Abort();
            receiveThread = null;
            receiveClient.Close();
            receiveClient = null;
            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    void Update()
    {
        if (receiveQueue.Count != 0)
        {
            float[] message;

            message = (float[])receiveQueue.Dequeue();

            if (message[1] < 0.2F)
            {
                //m_Material.color = Color.black;

                if (incinacion > -200000)
                {
                    cube.transform.Rotate(0, 0,-2);
                    incinacion -= 2;

                }
            }
            if (message[1] > -0.2F)
            {
                //m_Material.color = Color.red;

                if (incinacion < 200000)
                {
                    cube.transform.Rotate(0, 0,2);
                    incinacion += 2;
                }
            }
            //if (cX == 1 ) 
            //if (cX == 2) m_Material.color = Color.red;

        }

    }

}