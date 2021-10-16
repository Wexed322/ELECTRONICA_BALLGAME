using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO.Ports;

public class SerialCom : MonoBehaviour
{
    //Variables to connect to arduino
    bool isConnected = false;
    SerialPort port;
    string[] ports;
    public Dropdown lista;
    public string texto_entrada;
    string portname;
    public bool boton_Start_Desconectar = false;

    //Game variables
    public bool flag_muerte = false;
    public float contador;
    public float contador_muerte;
    public float fuerza;
    public float fuerza2;
    public float rotacion_z;
    public float rotacion_x;

    //Game UI 
    public Text contador_UI;
    public GameObject Start_UI;
    public GameObject Puertos_UI;
    public GameObject Salir_UI;
    public GameObject perdiste;
    public GameObject esfera;
    public GameObject breadboard;
    private Rigidbody rb;



    //Gameplay
    public string roll;
    public string pitch;
    public string button_up;

    private void Awake() {


        lista.options.Clear();
        ports = SerialPort.GetPortNames();

        foreach (string port in ports)
        {
            lista.options.Add(new Dropdown.OptionData() { text = port });
        }

        DropdownItemSelected(lista);

        lista.onValueChanged.AddListener(delegate { DropdownItemSelected(lista); });
    }

    public int conversao(string cadena)
    {
        int resul_final = 0;
        int longitu_palabra = cadena.Length;
        int[] arreglo_de_ints = new int[longitu_palabra];
        for (int i = 0; i < cadena.Length; i++)
        {
            arreglo_de_ints[i] = (int)char.GetNumericValue(cadena[i]);
            longitu_palabra -= 1;
            resul_final = ((int)Mathf.Pow(10, longitu_palabra)) * arreglo_de_ints[i] + resul_final;
        }
        return resul_final;
    }

    void DropdownItemSelected(Dropdown lista) {
        int indice = lista.value;
        portname = lista.options[indice].text;
    }
    
    public void conectar() {
        if (!isConnected)
        {
            connect_to_Arduino();
        }
    }

    void connect_to_Arduino()
    {
        isConnected = true;
        port = new SerialPort(portname, 9600, Parity.None, 8, StopBits.One);

        port.Open();
        port.Write("#STAR\n");

        this.Start_UI.gameObject.SetActive(false);
        this.Puertos_UI.gameObject.SetActive(false);
    }

    public void desconectar() {
        if(isConnected){ 
            disconnect_from_Arduino();
        }
        
        SceneManager.LoadScene("SampleScene");
        boton_Start_Desconectar = false;
        flag_muerte = false;
    }

    void disconnect_from_Arduino() {
        isConnected = false;
        port.Write("#STOP\n");
        port.Close();

        
    }

    public void readStrintInput(string s) {
        texto_entrada = s;
        Debug.Log(texto_entrada);
    }

    public void sendText() {
        if (isConnected) {
            port.WriteLine(texto_entrada);
            
        }
    }

    void Start() 
    {
        rb = esfera.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if (isConnected) 
        {
            roll = port.ReadLine();
            pitch = port.ReadLine();
            button_up = port.ReadLine();

            if (button_up == "UP") 
            {
                Debug.Log("UP");
                boton_Start_Desconectar = !boton_Start_Desconectar;
                if (boton_Start_Desconectar == false)
                {
                    SceneManager.LoadScene("SampleScene");
                    boton_Start_Desconectar = false;
                    flag_muerte = false;
                }
                else
                {
                    rb.isKinematic = false;
                    fuerza = Random.Range(-120, 120);
                    fuerza2 = Random.Range(-120, 120);
                    rb.AddForce(fuerza, 0, fuerza2, ForceMode.Impulse);
                }
                
            }
            if (button_up == "DOWN") 
            {
                Debug.Log("DOWN");
            }if (button_up == "NADA") 
            {
                Debug.Log("NADA");
            }
        }
        
        if (boton_Start_Desconectar == true) 
        {
            int z = conversao(roll);
            int x = conversao(pitch);
            Debug.Log(z);
            Debug.Log(x);
            breadboard.transform.rotation = Quaternion.Euler(x, 0, -z);

            if (flag_muerte == false) 
            {
                contador_funcion();
            }
            
        }


    }

    void contador_funcion()
    {
        contador += Time.deltaTime;
        contador_UI.text = ((int)contador).ToString();
    }
    
    
}
