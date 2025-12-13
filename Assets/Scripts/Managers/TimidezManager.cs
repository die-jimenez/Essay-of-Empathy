using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimidezManager : MonoBehaviour
{
    public static TimidezManager instance;
    IEnumerator[] ContadorBajarBarrera;
    public Puntero puntero;
    float protaPosXNormalized;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ContadorBajarBarrera = new IEnumerator[GameManager.instance.barras.Count];
        GameManager.instance.puntero.Pressed.AddListener(PunteroPressed);
        GameManager.instance.puntero.Released.AddListener(PunteroReleased);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(ColisionEnter);
        GameManager.instance.puntero.PunteroTriggerExit.AddListener(ColisionExit);
        protaPosXNormalized = Mathf.InverseLerp(-9, 9, GameManager.instance.protagonista.transform.position.x);
    }

    private void Update()
    {
        if (GameManager.instance.hayInterccion)
        {
            float punteroPosXNormalized = Mathf.InverseLerp(-9, 9, puntero.transform.position.x);
            float distPunteroToProta = Mathf.Abs(punteroPosXNormalized - protaPosXNormalized) * 10;

            if (distPunteroToProta > 0.5f)
            {
                GameManager.instance.protagonista.heightTarget = Remap(distPunteroToProta, 8.5f, -2.5f, 1.4f, -4.5f);
            }
            else
            {
                GameManager.instance.protagonista.heightTarget = -5.25f;
            }
        }

    }



    void PunteroPressed()
    {
        Barra prota = GameManager.instance.protagonista;
        prota.movimiento = Barra.Movimiento.manual;
        prota.GetValoresEstadoB();
    }

    void PunteroReleased()
    {
        Barra prota = GameManager.instance.protagonista;
        prota.movimiento = Barra.Movimiento.automatico;
        prota.direccion = Barra.Direccion.sube;
        prota.GetValoresEstadoA();
    }

    void ColisionEnter(Barra barraColisionada)
    {
        if (barraColisionada.estado == Barra.Estado.estado_A)
        {
            LevantarBarra(barraColisionada);
        }
        if (ContadorBajarBarrera[barraColisionada.index] != null)
        {
            StopCoroutine(ContadorBajarBarrera[barraColisionada.index]);
            ContadorBajarBarrera[barraColisionada.index] = null;
        }
        if(barraColisionada == GameManager.instance.protagonista)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }    
    }

    void ColisionExit(Barra barraColisionada)
    {
        if (ContadorBajarBarrera[barraColisionada.index] == null && barraColisionada != GameManager.instance.protagonista)
        {
            ContadorBajarBarrera[barraColisionada.index] = BajarBarrera(barraColisionada);
            StartCoroutine(ContadorBajarBarrera[barraColisionada.index]);
            //ContadorBajarBarrera[barraColisionada.index] = null;
        }
        if (barraColisionada == GameManager.instance.protagonista)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
        }
    }







    void LevantarBarra(Barra barraColisionada)
    {
        if (!barraColisionada.esProtagonista)
        {
            barraColisionada.GetValoresEstadoB();
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.velocidadSubida = 8f;

            barraColisionada.AtHighestPoint.AddListener(() =>
            {
                barraColisionada.GetValoresEstadoB();
                barraColisionada.AtHighestPoint.RemoveAllListeners();
            });
        }
    }

    IEnumerator BajarBarrera(Barra barraColisionada)
    {
        yield return new WaitForSeconds(3.5f);
        barraColisionada.GetValoresEstadoA();
        barraColisionada.direccion = Barra.Direccion.baja;
        barraColisionada.movimiento = Barra.Movimiento.automatico;
        barraColisionada.velocidadBajada = 8f;

        barraColisionada.AtLowestPoint.AddListener(() =>
        {
            barraColisionada.GetValoresEstadoA();
            barraColisionada.AtLowestPoint.RemoveAllListeners();
            ContadorBajarBarrera[barraColisionada.index] = null;
        });
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        float OldRange = (from2 - from1);
        float NewRange = (to2 - to1);
        return (((value - from1) * NewRange) / OldRange) + to1;
    }
}
