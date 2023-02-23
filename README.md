# INTERACTIVOS_2_REPARTIDOR

## Brainstorm
Comenzamos pensando diversas formas de llegar a entregar algo entretenido, educativo y que a su vez cumpliese las expectativas del curso, para cumplir todos estos objetivos se nos ocurrió crear un juego donde tengas que moverte a través de una ciudad en un coche respetando las reglas de tránsito, de esta manera conectamos de diferentes maneras diversos dispositivos a un computador, como por ejemplo un celular para tomar la inclinación del volante, una cámara live motion para hacer los cambios de marcha y un controlador para acelerar y frenar con los pies.
![FLive](https://mocap.reallusion.com/iclone-motion-live-mocap/includes/images/leapmotion/LeapMotion-Feature.jpg)
![FCarro](https://www.infobae.com/new-resizer/_JrNAvjIeELPSozYjXRd5qu_-nU=/1200x900/filters:format(webp):quality(85)//cloudfront-us-east-1.images.arcpublishing.com/infobae/M4TLYYA5CZDUFGQFUEHOLVYWSI.jpg)
![FSimulacion](https://http2.mlstatic.com/D_NQ_NP_891663-MCO45143406414_032021-O.jpg)

## Carlos Andrés Escobar López
Estuve trabajando principalmente con el fin de encontrar una manera de conectar las acciones de un cubo en Unity que respondiese a las señales que se les enviase, para esto probé este codigo proporcionado por el profesor: 
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class comm : MonoBehaviour
{

    private static comm instance;
    private Thread receiveThread;
    private UdpClient receiveClient;
    private IPEndPoint receiveEndPoint;
    public string ip = "0";
    public int receivePort = 32002;
    private bool isInitialized;
    private Queue receiveQueue;
    public GameObject cube;
    private Material m_Material;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        m_Material = cube.GetComponent<Renderer>().material;
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
                SerializeMessage(text);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }

    private void SerializeMessage(string message)
    {
        try
        {
            string[] chain = message.Split(' ');
            string key = chain[0];
            float value = 0;
            if (float.TryParse(chain[1], out value))
            {
                receiveQueue.Enqueue(value);
            }
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
            float counter = (float)receiveQueue.Dequeue();

            if (counter == 1F) m_Material.color = Color.black;
            if (counter == 2F) m_Material.color = Color.red;
        }

    }

}

con este logré hacer que el cubo cambiase de color al enviarle un mensaje a través de Hercules Terminal, sigo investigando la manera de enviar estos mensajes a través del celular.

![CuboRojo](Imagenes/CuboRojo.png)
![CuboRojo](Imagenes/CuboNegro.png)


Para lograr que el cubo recibiera algo más allá de únicamente ordenes de ponerse en rojo o negro, usamos una aplicación llamda zig zim, con ella le enviamos la información de un array llamado “gravity” para esto modificamos el codigo, y con esto le pedimos que al recibir valores dentro de ciertos márgenes el cubo se voltease.
