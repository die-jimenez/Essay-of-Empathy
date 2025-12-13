using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DesinteresManager : MonoBehaviour
{
    public static DesinteresManager instance;
    [Range(0, 1f)] public float diferenciaAltura;
    int indexBarraColisionada;
    public Puntero puntero;





    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        indexBarraColisionada = 0;
        GameManager.instance.puntero.Pressed.AddListener(() => GameManager.instance.CambiarFondo(GameManager.instance.fondoB));
        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
        GameManager.instance.puntero.PunteroTriggerStay.AddListener(ReorganizarBarras);
    }

    private void Update()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (!GameManager.instance.hayInterccion)
            {
                if (barra.esProtagonista)
                {
                    barra.GetValoresEstadoA();
                }
                else
                {
                    //Recien terminar la interaccion
                    if (barra.movimiento == Barra.Movimiento.manual)
                    {
                        barra.movimiento = Barra.Movimiento.automatico;
                        barra.direccion = Barra.Direccion.baja;
                        barra.GetValoresEstadoA();
                        barra.velocidadBajada = 10f;
                        barra.estado = Barra.Estado.transicion;
                        return;
                    }
                    //Tras la bajada acelerada al terminar la interaccion
                    if (barra.direccion == Barra.Direccion.sube && barra.estado == Barra.Estado.transicion)
                    {
                        barra.GetValoresEstadoA();
                    }
                }
            }
        }
    }


    void CollisionEnter(Barra barraColisionada)
    {
        GetIndexBarraColisionada(barraColisionada);
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoB();
            if (!barra.esProtagonista)
            {
                barra.movimiento = Barra.Movimiento.manual;
            }
        }
    }

    void GetIndexBarraColisionada(Barra barraColisionada)
    {
        indexBarraColisionada = GameManager.instance.barras.FindIndex(a => a == barraColisionada);
    }

    void ReorganizarBarras(Barra barraColisionada)
    {
        //Barra Principal
        GameManager.instance.barras[indexBarraColisionada].heightTarget = GameManager.instance.punteroPosition.y;

        //Barras a la izquierda
        for (int i = indexBarraColisionada-1; i >= 0; i--)
        {
            GameManager.instance.barras[i].heightTarget = GameManager.instance.punteroPosition.y - ((indexBarraColisionada-i) * diferenciaAltura);
        }
        //Barras a la derecha
        for (int i = indexBarraColisionada + 1; i < GameManager.instance.barras.Count; i++)
        {
            GameManager.instance.barras[i].heightTarget = GameManager.instance.punteroPosition.y   - ((i-indexBarraColisionada) * diferenciaAltura);
        }
    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

}
