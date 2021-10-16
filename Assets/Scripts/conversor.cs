using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conversor : MonoBehaviour
{
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
    void Start()
    {
        int c = conversao("322");
        Debug.Log(c+100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
