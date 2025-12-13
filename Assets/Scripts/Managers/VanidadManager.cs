using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanidadManager : MonoBehaviour
{
    public static VanidadManager instance;

    [Header("Barras elegidas")]
    public Barra[] barrasActivadas = new Barra[5];
    int indexBarraColisionada;





    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
        GameManager.instance.puntero.PunteroTriggerExit.AddListener(CollisionExit);
        //GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        //GameManager.instance.puntero.punteroMantieneColision.AddListener(CollisionStay);
    }


    private void Update()
    {
        //if (!GameManager.instance.hayInterccion)
        //{
        //    DesactivarBarrasActivas();
        //}
    }



    void CollisionEnter(Barra barraColisionada)
    {
        barraColisionada.GetValoresEstadoB();
        barraColisionada.movimiento = Barra.Movimiento.manual;
        barraColisionada.heightTarget = barraColisionada.alturaMax;

        if (barraColisionada == GameManager.instance.protagonista)
        {
            GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
        }
    }

    void CollisionExit(Barra barraColisionada)
    {
        barraColisionada.velocidadSubida = 10f;
        barraColisionada.direccion = Barra.Direccion.sube;
        barraColisionada.movimiento = Barra.Movimiento.automatico;

        barraColisionada.AtHighestPoint.AddListener(() =>
        {
            barraColisionada.GetValoresEstadoA();
            barraColisionada.velocidadBajada = 8f;
            if (barraColisionada != GameManager.instance.protagonista)
            {
                barraColisionada.tiempoSostenidoEnMaximo = 1.5f;
            }
            else barraColisionada.tiempoSostenidoEnMaximo = 3.5f;

            barraColisionada.AtHighestPoint.RemoveAllListeners();
        });




        


        


        barraColisionada.AtLowestPoint.AddListener(() =>
        {
            barraColisionada.GetValoresEstadoA();
            if (barraColisionada == GameManager.instance.protagonista)
            {
                GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
            }
            barraColisionada.AtLowestPoint.RemoveAllListeners();
        });
    }






    #region Funciones: Colsion Enter
    //void CollisionEnter(Barra barraColisionada)
    //{
    //    DesactivarBarrasActivas();
    //    GetBarrasActivas(barraColisionada);
    //    foreach (Barra barraActivada in barrasActivadas)
    //    {
    //        barraActivada.heightTarget = barraActivada.data.alturaMax_B;
    //        barraActivada.movimiento = Barra.Movimiento.manual;
    //        barraActivada.GetValoresEstadoB();

    //        if (barraActivada == GameManager.instance.protagonista) GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
    //    }
    //}

    //void GetBarrasActivas(Barra barra)
    //{
    //    List<Barra> _barras = GameManager.instance.barras;
    //    indexBarraColisionada = _barras.FindIndex(a => a.gameObject == barra.gameObject);
    //    //Dos barras a la izquierda de la barra colsionada
    //    if (indexBarraColisionada - 2 >= 0) barrasActivadas[0] = _barras[indexBarraColisionada - 2];
    //    else barrasActivadas[0] = _barras[indexBarraColisionada];
    //    //Una barras a la izquierda de la barra colsionada
    //    if (indexBarraColisionada - 1 >= 0) barrasActivadas[1] = _barras[indexBarraColisionada - 1];
    //    else barrasActivadas[1] = _barras[indexBarraColisionada];
    //    //Barra colisionada
    //    barrasActivadas[2] = _barras[indexBarraColisionada];
    //    //Una barras a la derecha de la barra colsionada
    //    if (indexBarraColisionada + 1 < _barras.Count) barrasActivadas[3] = _barras[indexBarraColisionada + 1];
    //    else barrasActivadas[3] = _barras[indexBarraColisionada];
    //    //Dos barras a la derecha de la barra colsionada
    //    if (indexBarraColisionada + 2 < _barras.Count) barrasActivadas[4] = _barras[indexBarraColisionada + 2];
    //    else barrasActivadas[4] = _barras[indexBarraColisionada];
    //}

    //void DesactivarBarrasActivas()
    //{
    //    foreach (Barra barra in GameManager.instance.barras)
    //    {
    //        //Recien terminar la interaccion... sigue subiendo o se queda sostenido
    //        if (barra.movimiento == Barra.Movimiento.manual)
    //        {
    //            barra.movimiento = Barra.Movimiento.automatico;
    //            barra.direccion = Barra.Direccion.sube;
    //            barra.estado = Barra.Estado.transicion;
    //            if (barra.esProtagonista) barra.tiempoSostenidoEnMaximo = 4.5f;
    //            else barra.tiempoSostenidoEnMaximo = 1.5f;
    //        }
    //        //Tras el tiempo sostendo en la altura maxima tras terminar la interaccion
    //        if (barra.direccion == Barra.Direccion.baja && barra.estado == Barra.Estado.transicion)
    //        {
    //            barra.GetValoresEstadoA();
    //            barra.velocidadBajada = 5;
    //        }
    //        //Tras la bajada acelerada al terminar la interaccion
    //        if (barra.direccion == Barra.Direccion.sube && barra.estado == Barra.Estado.estado_A)
    //        {
    //            barra.GetValoresEstadoA();
    //        }
    //    }
    //}
    #endregion

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
