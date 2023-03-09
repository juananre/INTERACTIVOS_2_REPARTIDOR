# INTERACTIVOS_2_REPARTIDOR

## Prototipo #1

Brainstorm
Comenzamos pensando diversas formas de llegar a entregar algo entretenido, educativo y que a su vez cumpliese las expectativas del curso, para cumplir todos estos objetivos se nos ocurrió crear un juego donde tengas que moverte a través de una ciudad en un coche respetando las reglas de tránsito, de esta manera conectamos de diferentes maneras diversos dispositivos a un computador, como por ejemplo un celular para tomar la inclinación del volante, una cámara live motion para hacer los cambios de marcha y un controlador para acelerar y frenar con los pies.

![FLive](https://mocap.reallusion.com/iclone-motion-live-mocap/includes/images/leapmotion/LeapMotion-Feature.jpg)
![FCarro](https://www.infobae.com/new-resizer/_JrNAvjIeELPSozYjXRd5qu_-nU=/1200x900/filters:format(webp):quality(85)//cloudfront-us-east-1.images.arcpublishing.com/infobae/M4TLYYA5CZDUFGQFUEHOLVYWSI.jpg)
![FSimulacion](https://http2.mlstatic.com/D_NQ_NP_891663-MCO45143406414_032021-O.jpg)

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
## Prototipo #2
con este logré hacer que el cubo cambiase de color al enviarle un mensaje a través de Hercules Terminal, sigo investigando la manera de enviar estos mensajes a través del celular.

![CuboRojo](Imagenes/CuboRojo.png)
![CuboRojo](Imagenes/CuboNegro.png)
Para lograr que el cubo recibiera algo más allá de únicamente ordenes de ponerse en rojo o negro, usamos una aplicación llamda zig zim, con ella le enviamos la información de un array llamado “gravity” para esto modificamos el codigo, y con esto le pedimos que al recibir valores dentro de ciertos márgenes el cubo se voltease.

[(Giro del cubo)](https://www.youtube.com/watch?v=OpWp2a5FJ08)

Ahora que se mueve hicimos un playerController, el cual tiene como función mover el cubo hacia adelante y agregarle un collider, el cual le proporcionó la gravedad a el cubo y la posibilidad de colisionar con diferentes objetos del escenario.

[(Movimiento del cubo)](https://youtu.be/MATfODdEWHo)

Se hizo un modelo del carro lowpoly, que no consuma muchos recursos ya que probando assets de vehiculos realistas consumia demasiado, tuvimos unos problemas breves exportando los modelos a unity pero se logor solucionar para que quedara como deberia.

![image](https://media.discordapp.net/attachments/1076728843009790045/1080944040599629964/IMG-20230302-WA0012.jpg)

Ya con esto funcionando tomamos, el cubo y lo reemplazamos por un asset de un vehículo, y un escenario de una pequeña ciudad el cual le agregó mucho movimiento, a demás de hacer posible que el vehículo se pueda encender, apagar, tener 5 cambios diferentes a demás de acelerar y desacelerar dependiendo de como utilices el botón destinado para ello

[(Carro en movimiento)](https://youtu.be/uHbwmQEYOUw)

Las mecanicas ya estan planteadas para implementarse, tanto las del usuario como las de los obstaculos y mundo; y el mapeado tambien esta planteado, falta la construccion y definir assets que se van a usar


## leap

implemente el funcionamiento del leap pero aun no puedo hacer prubas en implamentes una zona de pruebas donde las personas puedadn trabajar al igual que las ramificaciones. un ma dificialta es el que no pude encontrar una palaca de cambios y por eso decidi modelarda.

![Captura3](https://user-images.githubusercontent.com/78058130/217722809-e8744dde-9925-4866-99c4-b51ab8279f28.JPG)
![Captura2](https://user-images.githubusercontent.com/78058130/217722832-edf28ea2-7f46-408b-9491-d7cb900d5c6c.JPG)
![Captura](https://user-images.githubusercontent.com/78058130/217722841-a33356e9-237a-49d8-bfad-a8cb4b286658.JPG)


Al momento de adaptar las manos con la interacción hubo un error que rompía el rig de las manos, no permitiendo interacciones 
![image](https://user-images.githubusercontent.com/78058130/220830084-4c852f87-e74d-4910-8734-00e3268e4df0.png)
pero se logró corregir remplazando por otro modelo de malos.
en cuanto a la palanca se mantiene funcionando pero requiere de un rig propio para que no se dañe la caja de cambios

## Avance 3.1

Encontramos un error el cual no permitía comunicar el Leap y el teclado al tiempo, esta incompatibilidad se debía a que estábamos utilizando “(Input.GetKeyDown(KeyCode.W))” tanto en los botones del Leap como en los del teclado lo que generaba incompatibilidades dado a que al presionar el boton no se daba la acción esperada, introducimos dos cajas que hagan la función de botones, y así poder controlar los cambios del vehículo a partir del Leap.

![CubosCambios](Imagenes/Cubos.png)

Se tomaron las medidas y planteamos lo materiales que se usaran para el montaje, en el que al inicio planeabamos hacerlo con tubos PVC por su alta resistencia pero seria muy costoso, por lo que al final se decidio que usariamos madera ya que tenemos las herramientas para trabajar con madera y es mas flexible, el unico problema que tendremos con un montaje en madera es el transporte, ya que no se puede desensamblar las piezas para transportarlo, al contrario de PVC. Se plantea que se movera la silla para acomodar a los diferentes usuarios con un mecanismo parecido al de los vehiculos. Se le realizaron unos planos y un modelo 3D al montaje para tener la idea, esta planeado comprar los materiales y empezar a hacerlo para la proxima semana.

![image](https://media.discordapp.net/attachments/1076728843009790045/1080943698352808027/IMG-20230302-WA0007.jpg)
![image](https://media.discordapp.net/attachments/1076728843009790045/1080943782272454747/IMG-20230302-WA0008.jpg?width=924&height=625)

El mapa esta en proceso, cerca de terminarse para empezar a implementar los anteriores avances para estar montando todo.

## Avance 3.2

El montaje esta planeado y tenemos los materiales en el taller listo para empezar a trabajarlo, es en madera ya que es lo mas economico y estable que podemos lograr.

El mapa ya tiene todas las vias que es el lugar donde estara el usuario disfrutando de la experiencia, falta sus edificios y decoraciones para que quede listo, y las mecanicas que usaremos para el mapa ya estan planteadas pero tenemos que implementarlas y probar; Y arreglamos el boton que se usara para los cambios del carro, que quede mejor para el leap.

