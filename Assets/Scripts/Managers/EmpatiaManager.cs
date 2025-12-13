using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpatiaManager : MonoBehaviour
{
    public static EmpatiaManager instance;
    [Header("Barras elegidas")]
    int indexBarraColisionada;
    bool barrasLevantadas;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.Pressed.AddListener(() => StartCoroutine(LevantarBarras()));
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
        GameManager.instance.puntero.Released.AddListener(PunteroReleased);
    }


    void CollisionEnter(Barra barraColisionada)
    {
        CongelarProtagonista();
        BajarBarreras(barraColisionada);
    }

    void PunteroReleased()
    {
        RegresarEstadoInicial();
        CancelarCorrutinas();
        barrasLevantadas = false;
    }


    void CongelarProtagonista()
    {
        GameManager.instance.protagonista.movimiento = Barra.Movimiento.manual;
        GameManager.instance.protagonista.heightTarget = GameManager.instance.protagonista.transform.position.y-0.5f;
    }

    void BajarBarreras(Barra barraColisionada)
    {
        if (barrasLevantadas)
        {
            if (!barraColisionada.esProtagonista)
            {
                barraColisionada.GetValoresEstadoB();
                barraColisionada.movimiento = Barra.Movimiento.manual;
                barraColisionada.heightTarget = GameManager.instance.protagonista.transform.position.y;
            }
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }
    }

    void RegresarEstadoInicial()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoA();
            barra.direccion = Barra.Direccion.baja;
            barra.movimiento = Barra.Movimiento.automatico;
            barra.velocidadBajada = 6f;

            barra.AtLowestPoint.AddListener(() =>
            {
                barra.GetValoresEstadoA();
                barra.AtLowestPoint.RemoveAllListeners();
            });
        }
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }


    IEnumerator LevantarBarras()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (barra != GameManager.instance.protagonista)
            {
                barra.GetValoresEstadoB();
                barra.movimiento = Barra.Movimiento.manual;
                barra.heightTarget = barra.alturaMax;
            }
        }
        yield return new WaitForSeconds(0.4f);
        barrasLevantadas = true;
    }

    void CancelarCorrutinas()
    {
        StopCoroutine("LevantarBarras");
    }
}
