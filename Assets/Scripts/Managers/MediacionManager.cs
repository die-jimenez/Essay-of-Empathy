using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediacionManager : MonoBehaviour
{
    public static MediacionManager instance;
    int indexBarraColisionada;
    int protaIndex;

    List<Barra> leftBarras = new List<Barra>();
    List<Barra> rightBarras = new List<Barra>();




    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        indexBarraColisionada = 0;
        protaIndex = GameManager.instance.barras.FindIndex(a => a == GameManager.instance.protagonista);
        Debug.Log(protaIndex);

        for (int i = 0; i < GameManager.instance.barras.Count; i++)
        {
            if (i < protaIndex) leftBarras.Add(GameManager.instance.barras[i]);
            else if (i > protaIndex) rightBarras.Add(GameManager.instance.barras[i]);
        }

        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
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
            barra.movimiento = Barra.Movimiento.manual;
        }
        ReorganizarBarras();
        GameManager.instance.protagonista.heightTarget = -1;
    }

    void GetIndexBarraColisionada(Barra barraColisionada)
    {
        indexBarraColisionada = GameManager.instance.barras.FindIndex(a => a == barraColisionada);
    }

    void ReorganizarBarras()
    {
        float alturaMaximaGrupal = 5;
        float factorAltura = Mathf.Abs(indexBarraColisionada - protaIndex) +5;

        if (indexBarraColisionada < protaIndex)
        {
            foreach (Barra barra in leftBarras) barra.heightTarget = (factorAltura * (alturaMaximaGrupal / GameManager.instance.barras.Count))-1;
            foreach (Barra barra in rightBarras) barra.heightTarget = (-factorAltura * (alturaMaximaGrupal / GameManager.instance.barras.Count))-1;
            RegresarFondoA();
        }
        else if (indexBarraColisionada > protaIndex)
        {
            foreach (Barra barra in leftBarras) barra.heightTarget = (-factorAltura * (alturaMaximaGrupal / GameManager.instance.barras.Count))-1;
            foreach (Barra barra in rightBarras) barra.heightTarget = (factorAltura * (alturaMaximaGrupal / GameManager.instance.barras.Count)) -1;
            RegresarFondoA();
        }
        else if (indexBarraColisionada == protaIndex)
        {
            foreach (Barra barra in GameManager.instance.barras) barra.heightTarget = -1;
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }

    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
