using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte2 : MonoBehaviour
{
    public SerialCom serialScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        serialScript.flag_muerte = true;
        Muerte();
    }
    void Muerte()
    {
        Destroy(serialScript.esfera);
        serialScript.perdiste.gameObject.SetActive(true);
        serialScript.Salir_UI.gameObject.SetActive(true);
        serialScript.Salir_UI.gameObject.transform.position = new Vector3(300, 60, 0);
    }
}
