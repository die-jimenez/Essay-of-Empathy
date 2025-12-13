using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum Concepto { Acoso, Desamparo, Desinteres, Empatia, Mediacion, Proteccion, Timidez, Vanidad, Xenofobia };


    [Header("General")]
    public List<Barra> barras = new List<Barra>();
    public Concepto concepto;
    public Barra protagonista;
    public Color fondoA;
    public Color fondoB;

    [Header("Puntero - Modo manual")]
    public Puntero puntero;
    public Vector2 punteroPosition;
    public bool hayInterccion;

    [Header("Organizador")]
    [Range(-8.28f, 8.28f)] public float posicionXPrimeraBarra;
    [Range(-0.2f, 0.2f)] public float separacionHorizontal;
    



    private void Awake()
    {
        instance = this;
        //2000x1200
        posicionXPrimeraBarra = -7.7f;
        separacionHorizontal = -0.093f;
        OrganizarHorizontalmente();

#if UNITY_EDITOR
        //Medidas 16:9
        //posicionXPrimeraBarra = -8.28f;
        //separacionHorizontal = 0.078f;
#endif

#if UNITY_ANDROID
        //posicionXPrimeraBarra = -7.7f;
        //separacionHorizontal = -0.093f;
#endif
    }

    private void Start()
    {
        posicionXPrimeraBarra = -8.28f;
        separacionHorizontal = 0.315f;
        Application.targetFrameRate = 30;
        fondoA = Camera.main.backgroundColor;
    }



    [Button(Spacing = ButtonSpacing.Before)]
    void OrganizarHorizontalmente()
    {
        float nuevaPosicion = posicionXPrimeraBarra;
        foreach (Barra barra in barras)
        {
            barra.transform.position = new Vector3(nuevaPosicion, barra.transform.position.y, barra.transform.position.z);
            nuevaPosicion += barra.transform.localScale.x + separacionHorizontal;
        }
    }

    public void CambiarFondo(Color newColor)
    {
        Camera.main.backgroundColor = newColor;
    }


    /*
    [Button]
    void BuscarBarras()
    {
        //GameObject[] barraEncontradas = GameObject.FindObjectsByType<MovimientoVertical>();
    }
    */
    /*
    //[Header("Settear Ola")]
    //[SerializeField] GameObject ultimaBarraIzquierda;
    //[SerializeField] GameObject ultimaBarraDerecha;
    //[Tooltip("Entre mayor sea, menor es el ancho de las ondas")]
    //[SerializeField, Range(0, 1)] float frecuenciaX;
    //[Tooltip("Entre mayor sea, mayor el alto de las ondas")]
    //[SerializeField, Range(0, 1)] float magnitudY;
     
    [Button]
    void SetPositionsToWave()
    {
        //Codigo sacado de: https://www.youtube.com/watch?v=rHh43ilfnyI&t=110s
        //Recomendacion: Prueba la funcion aqui: https://www.geogebra.org/graphing?lang=es-AR
        foreach (GameObject barra in barras)
        {
            //Se mueve con "sin"
            //float posY = barra.transform.position.x * Mathf.Sin(frecuenciaX) * magnitudY;
            float posY = Mathf.Sin(barra.transform.position.x * frecuenciaX) * magnitudY;
            barra.transform.position = new Vector3(barra.transform.position.x, posY, barra.transform.position.z);
            //Settear alturas minimas y maximas
            MovimientoVertical barraScript = barra.GetComponent<MovimientoVertical>();
            barraScript.tiempoSostenidoEnMinimo = 0;
            barraScript.tiempoSostenidoEnMaximo = 0;
            barraScript.rangoVariacionAltura = 0;
            barraScript.alturaMin = -5.5f;
            barraScript.alturaMax = 3.5f;

        }




        if (eleccionAleatoriaDeMovimiento == 1)//Se mueve con "Cos"
        {
            nuevaPosicion = position + transform.up * Mathf.Cos(Time.time * frecuenciaX) * magnitudY;
        }
        rBody.MovePosition(nuevaPosicion);
    }
    */

}
