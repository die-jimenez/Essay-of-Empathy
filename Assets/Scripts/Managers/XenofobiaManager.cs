using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XenofobiaManager : MonoBehaviour
{
    public static XenofobiaManager instance;
    IEnumerator[] ContadorBajarBarrera;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ContadorBajarBarrera = new IEnumerator[GameManager.instance.barras.Count];
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(ColisionEnter);
        GameManager.instance.puntero.PunteroTriggerExit.AddListener(ColisionExit);

        GameManager.instance.puntero.Released.AddListener(PunteroReleased);
    }

    private void Update()
    {

    }


    void PunteroReleased()
    {
        //RegresarFondoA();
    }

    void ColisionEnter(Barra barraColisionada)
    {
        //ActivarBarras(barraColisionada);

        if (barraColisionada.estado == Barra.Estado.estado_A)
        {
            ActivarBarras(barraColisionada);
        }

        if (barraColisionada.index != 1 || barraColisionada.index != 2)
        {
            if (ContadorBajarBarrera[barraColisionada.index] != null)
            {
                StopCoroutine(ContadorBajarBarrera[barraColisionada.index]);
                ContadorBajarBarrera[barraColisionada.index] = null;
            }
        }

        if (barraColisionada == GameManager.instance.protagonista)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }
    }

    void ColisionExit(Barra barraColisionada)
    {
        //Barras muros
        if (barraColisionada.index == 1 || barraColisionada.index == 2)
        {
            barraColisionada.velocidadSubida = 10f;
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.movimiento = Barra.Movimiento.automatico;

            barraColisionada.AtHighestPoint.AddListener(() =>
            {
                BajarMuros(barraColisionada);
                barraColisionada.AtHighestPoint.RemoveAllListeners();
            });


        }
        //El resto de barras
        else
        {
            if (ContadorBajarBarrera[barraColisionada.index] == null)
            {
                ContadorBajarBarrera[barraColisionada.index] = BajarBarrera(barraColisionada);
                StartCoroutine(ContadorBajarBarrera[barraColisionada.index]);
                BajarMuros(barraColisionada);
            }
        }



    }



    void ActivarBarras(Barra barraColisionada)
    {
        if (barraColisionada.index == 1 || barraColisionada.index == 2)
        {
            barraColisionada.GetValoresEstadoB();
            barraColisionada.movimiento = Barra.Movimiento.manual;
            barraColisionada.heightTarget = barraColisionada.alturaMax;
            return;
        }
        else
        {
            if (barraColisionada.esProtagonista)
            {
                GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
            }

            barraColisionada.GetValoresEstadoB();
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.velocidadSubida = 4f;

            barraColisionada.AtHighestPoint.AddListener(() =>
            {
                barraColisionada.GetValoresEstadoB();
                barraColisionada.AtHighestPoint.RemoveAllListeners();
            });
        }
    }

    void BajarMuros(Barra barraColisionada)
    {
        if (barraColisionada.index == 1 || barraColisionada.index == 2)
        {
            barraColisionada.GetValoresEstadoA();
            barraColisionada.velocidadBajada = 6f;
            barraColisionada.tiempoSostenidoEnMaximo = 3f;
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.movimiento = Barra.Movimiento.automatico;

            barraColisionada.AtLowestPoint.AddListener(() =>
            {
                RegresarFondoA();
                barraColisionada.GetValoresEstadoA();
                barraColisionada.AtLowestPoint.RemoveAllListeners();
            });
        }
    }

    IEnumerator BajarBarrera(Barra barraColisionada)
    {
        yield return new WaitForSeconds(2f);
        barraColisionada.GetValoresEstadoA();
        barraColisionada.direccion = Barra.Direccion.baja;
        barraColisionada.movimiento = Barra.Movimiento.automatico;
        barraColisionada.velocidadBajada = 3f;

        barraColisionada.AtLowestPoint.AddListener(() =>
        {
            barraColisionada.GetValoresEstadoA();
            barraColisionada.AtLowestPoint.RemoveAllListeners();
            ContadorBajarBarrera[barraColisionada.index] = null;
        });
    }


    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
