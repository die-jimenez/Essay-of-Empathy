using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteccionManager : MonoBehaviour
{
    public static ProteccionManager instance;
    [Header("Barras Protectoras")]
    public Barra[] barrasProtectoras;

    [Header("Barras elegidas")]
    public Barra[] barrasActivadas = new Barra[2];
    int indexBarraColisionada;





    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        barrasActivadas = new Barra[3];
        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
    }


    private void Update()
    {
        if (!GameManager.instance.hayInterccion)
        {
            foreach (Barra barra in GameManager.instance.barras)
            {
                if (barra == barrasProtectoras[0] || barra == barrasProtectoras[1])
                {
                    if (barra.movimiento == Barra.Movimiento.manual)
                    {
                        barra.movimiento = Barra.Movimiento.automatico;
                        barra.direccion = Barra.Direccion.baja;
                        barra.estado = Barra.Estado.transicion;
                        barra.velocidadBajada = 8f;
                    }
                    else if (barra.movimiento == Barra.Movimiento.manual || barra.direccion == Barra.Direccion.sube)
                    {
                        barra.GetValoresEstadoA();
                    }
                }
                else if (barra != GameManager.instance.protagonista)
                {
                    if (barra.movimiento == Barra.Movimiento.manual)
                    {
                        barra.movimiento = Barra.Movimiento.automatico;
                        barra.direccion = Barra.Direccion.sube;
                        barra.estado = Barra.Estado.transicion;
                        barra.velocidadSubida = 4f;
                    }
                    else if (barra.movimiento == Barra.Movimiento.manual || barra.direccion == Barra.Direccion.baja)
                    {
                        barra.GetValoresEstadoA();
                    }
                }
            }
        }
    }



    void CollisionEnter(Barra barraColisionada)
    {
        GetBarrasActivas(barraColisionada);
        foreach (Barra barraActivada in barrasActivadas)
        {
            if (barraActivada.estado == Barra.Estado.estado_A)
            {
                if (barraActivada == barrasProtectoras[0] || barraActivada == barrasProtectoras[1])
                {
                    foreach (Barra barraProtectora in barrasProtectoras)
                    {
                        barraProtectora.GetValoresEstadoB();
                        barraProtectora.movimiento = Barra.Movimiento.manual;
                        barraProtectora.heightTarget = barraProtectora.alturaMax;
                    }
                    GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
                }
                else if (barraActivada != GameManager.instance.protagonista)
                {
                    barraActivada.GetValoresEstadoB();
                    barraActivada.movimiento = Barra.Movimiento.manual;
                    barraActivada.heightTarget = barraActivada.alturaMin;
                }
            }
        }
    }

    void GetBarrasActivas(Barra barra)
    {
        List<Barra> _barras = GameManager.instance.barras;
        indexBarraColisionada = _barras.FindIndex(a => a.gameObject == barra.gameObject);
        //Una barras a la izquierda de la barra colsionada
        if (indexBarraColisionada - 1 >= 0) barrasActivadas[0] = _barras[indexBarraColisionada - 1];
        else barrasActivadas[0] = _barras[indexBarraColisionada];
        //Barra colisionada
        barrasActivadas[1] = _barras[indexBarraColisionada];
        //Una barras a la derecha de la barra colsionada
        if (indexBarraColisionada + 1 < _barras.Count) barrasActivadas[2] = _barras[indexBarraColisionada + 1];
        else barrasActivadas[2] = _barras[indexBarraColisionada];
    }

    void ActivarBarrasDefensivas()
    {

    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

}
