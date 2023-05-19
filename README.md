
# Los Repartidores

![baner_sistemas](https://github.com/juananre/INTERACTIVOS_2_REPARTIDOR/assets/78058130/877eecbe-5c67-4cff-a330-01de991bdf4c)

Somos el equipo Triple A y queremos desarrollar 'LOS REPARTIDORES', una experiencia interactiva para enseñar lo básico de manejar a través de ser un repartidor. Nuestro objetivo es brindar una herramienta educativa para aquellos interesados practicar sus habilidades en manejo. 'LOS REPARTIDORES'.

## Proceso de diseño

## Tutorial

### 1. Heramientas a utilizar.

![logos](https://github.com/juananre/INTERACTIVOS_2_REPARTIDOR/assets/78058130/e7908bfe-baed-4f16-8183-0e1eef522341)


Para realizar este proyecto, lo primero que haremos es descargar las herramientas de trabajo. En primer lugar, **Unity**, que es el motor gráfico que utilizamos en la versión 2021.3.8f1 LTS **(puedes elegir la versión que prefieras)**. A continuación, crearemos un nuevo proyecto, al que llamaremos "Interactivos 2" **(puedes elegir el nombre que desees)**. Para tener un control de versiones, utilizaremos la herramienta GitHub y su extensión GitHub Desktop. Recuerda crear un archivo ".gitattributes" para evitar archivos innecesarios.

Luego, descargaremos la aplicación Sig Sim en el celular, ya que nos permitirá utilizar el giroscopio del dispositivo. También utilizaremos la aplicación Script Communicator para realizar pruebas de comunicación.

Antes de empezar a programar, es importante mantener un orden en todo y, al crear ramas en GitHub, trabajar en distintas escenas de Unity. Ahora sí, estamos listos para comenzar a programar.

### 2. Programación con celular.

Lo preimero sera conectar con Unity con el celualar. Para lograr hacer un juego como este, lo primero que tienes que hacer es crear un server al cual llegarán todos los inputs, en este caso tomaremos como base este código el cual es de mucha ayuda:

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


Este codigo se encarga de cambiar el color de un cubito dependiendo de la información que le llegue, si le llega un “1F” se hará negro y si le llega un “2F” se hará rojo, el codigo debe ponerse en un objeto en blanco y luego este objeto conectarlo a un cubito a través del 

“public GameObject cube;” 

![cubo](https://github.com/juananre/INTERACTIVOS_2_REPARTIDOR/assets/78058130/7be27e49-8962-4d06-9ec8-86a3f194e31f)

Esto enviándole la información desde una terminal como ScriptCommunicator el cual funciona como Cliente ante el servidor antes mencionado.

Para lograr que el cubo recibiera algo más allá de únicamente órdenes de ponerse en rojo o negro, usamos una aplicación llamada zig zim, con ella le enviamos la información de un array llamado “gravity” para esto modificamos el código, y con esto le pedimos que al recibir valores dentro de ciertos márgenes el cubo se voltease.

para poder enviar esta información requieres conectarte a través de un hotspot (compartiendo datos desde un celular) y luego tomar el IPV4 desde CMD escribiendo  “ ipconfig “ y desactivar el firewall.

![barra de sistemas](https://github.com/juananre/INTERACTIVOS_2_REPARTIDOR/assets/78058130/951e7c5b-c055-487d-809c-0a51851ec7e4)

[Un ejemplo de como deberia quedar](https://www.youtube.com/watch?v=OpWp2a5FJ08)

Ahora que se mueve hicimos un playerController, el cual tiene como función mover el cubo hacia adelante y agregarle un collider, el cual le proporcionó la gravedad al cubo y la posibilidad de colisionar con diferentes objetos del escenario.

[Asi fue como nos quedo](https://www.youtube.com/watch?v=MATfODdEWHo)

Se consiguió un modelo del carro lowpoly en Unity asset store, que no consume muchos recursos ya que probando assets de vehículos realistas consumía demasiado, tuvimos unos problemas breves exportando los modelos a unity pero se logró solucionar para que quedara como debería.

![carrolowpolly](https://github.com/juananre/INTERACTIVOS_2_REPARTIDOR/assets/78058130/725155a7-e4f4-4d4a-8c5d-9d4e00a5c9fa)

Ya con esto funcionando tomamos, el cubo y lo reemplazamos por un asset de un vehículo, y un escenario de una pequeña ciudad, el cual le agregó mucho movimiento, además de hacer posible que el vehículo se pueda encender, apagar, tener 5 cambios diferentes además de acelerar y desacelerar dependiendo de como utilices el botón destinado para ello.

[Y asi fue como nos quedo la implementacion del celular](https://www.youtube.com/watch?v=uHbwmQEYOUw)

### 3. Programación con Arduino.

### 4. Programación con leap.

### 5. Montaje.

### 6. Mecánicas.

### 7. Master.
