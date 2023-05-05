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
    // zigsim
    private Thread receiveThread;
    private UdpClient receiveClient;
    private IPEndPoint receiveEndPoint;
    public string ip = "127.0.0.1";
    public int receivePort = 6444;
    private bool isInitialized;
    private Queue receiveQueue;
    //Master
    private Thread masterThread;
    private UdpClient receiveMasterClient;
    private IPEndPoint receiveMasterEndPoint;
    public string ipMaster = "127.0.0.1";
    public int receiveMasterPort = 6000; 
    private bool isMasterInitialized;
    private Queue receiveMasterQueue;
    //Objeto que modifica
    public GameObject cube;

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
        //Zigsim
        instance = this;
        receiveEndPoint = new IPEndPoint(IPAddress.Parse(ip), receivePort); //recibe la informacion
        receiveClient = new UdpClient(receivePort);
        receiveQueue = Queue.Synchronized(new Queue()); //puerta
        receiveThread = new Thread(new ThreadStart(ReceiveDataListener)); //Crear nuevo hilo
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;
        //Master

        receiveMasterEndPoint = new IPEndPoint(IPAddress.Parse(ipMaster), receiveMasterPort); //recibe la informacion
        receiveMasterClient = new UdpClient(receiveMasterPort);
        receiveMasterQueue = Queue.Synchronized(new Queue()); //puerta
        masterThread = new Thread(new ThreadStart(ReciveMasterDataListener)); //Crear nuevo hilo
        masterThread.IsBackground = true;
        masterThread.Start();
        isMasterInitialized = true;
    }
    private void ReciveMasterDataListener()
    {
        while(true) 
        {
            try
            {
                
                byte[] dataMaster = receiveMasterClient.Receive(ref receiveMasterEndPoint);
                
                string textMaster = Encoding.UTF8.GetString(dataMaster);
       
                SerializeMasterMessage(textMaster);
                

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

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
               
                float[] coordenadas = (new float[] { zigSimdata.sensordata.gravity.x, zigSimdata.sensordata.gravity.y, zigSimdata.sensordata.gravity.z });

                SerializeMessage(coordenadas);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
    private void SerializeMasterMessage(string mastermenssage)
    {
        try
        {
            receiveMasterQueue.Enqueue((System.Object)mastermenssage);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void SerializeMessage(float[] menssage)
    {
        try
        {

            receiveQueue.Enqueue((System.Object)menssage);
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
        if (isMasterInitialized)
        {
            //master
            masterThread.Abort();
            masterThread = null;
            receiveMasterClient.Close();
            receiveMasterClient = null;
            Debug.Log("Thread Master killed");
            isMasterInitialized = false;
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
                if (incinacion > -200000)
                {
                    cube.transform.Rotate(0, 0,-2);
                    incinacion -= 2;

                }
            }
            if (message[1] > -0.2F)
            {

                if (incinacion < 200000)
                {
                    cube.transform.Rotate(0, 0,2);
                    incinacion += 2;
                }
            }

        }
        if (receiveMasterQueue.Count != 0)
        {
            string mastermenssage;
            mastermenssage = (string)receiveMasterQueue.Dequeue();
            Debug.Log(mastermenssage);
        }
    }

}