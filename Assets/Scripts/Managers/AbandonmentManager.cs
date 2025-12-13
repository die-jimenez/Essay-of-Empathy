using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonmentManager : MonoBehaviour
{
    public static AbandonmentManager instance;
    [Header("Selected Bars")]
    public Barra[] activatedBars;
    private int _collidedBarIndex;





    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        activatedBars = new Barra[9];
        IEnumerator desactivateActiveBars = DesactivateActiveBars();
        GameManager.instance.puntero.Released.AddListener(() => StartCoroutine(desactivateActiveBars));
        GameManager.instance.puntero.Pressed.AddListener(() => StopCoroutine(desactivateActiveBars));
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(CollisionEnter);
    }

    private void CollisionEnter(Barra collidedBar)
    {
        GetActiveBars(collidedBar);
        foreach (Barra activatedBar in activatedBars)
        {
            if (activatedBar.estado == Barra.Estado.estado_A)
            { 
                if (activatedBar != GameManager.instance.protagonista)
                {
                    activatedBar.GetValoresEstadoB();
                    activatedBar.movimiento = Barra.Movimiento.manual;
                    activatedBar.heightTarget = activatedBar.alturaMin;
                }
                else
                {
                    activatedBar.GetValoresEstadoB();
                    activatedBar.direccion = Barra.Direccion.baja;
                }
            }
        }
        GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
    }

    private void GetActiveBars(Barra bar)
    {
        List<Barra> _bars = GameManager.instance.barras;
        _collidedBarIndex = _bars.FindIndex(a => a.gameObject == bar.gameObject);
        //Four bars to the left of the collided bar
        if (_collidedBarIndex - 4 >= 0) activatedBars[0] = _bars[_collidedBarIndex - 4];
        else activatedBars[0] = _bars[_collidedBarIndex];
        //Three bars to the left of the collided bar
        if (_collidedBarIndex - 3 >= 0) activatedBars[1] = _bars[_collidedBarIndex - 3];
        else activatedBars[0] = _bars[_collidedBarIndex];
        //Two bars to the left of the collided bar
        if (_collidedBarIndex - 2 >= 0) activatedBars[2] = _bars[_collidedBarIndex - 2];
        else activatedBars[0] = _bars[_collidedBarIndex];
        //One bar to the left of the collided bar
        if (_collidedBarIndex - 1 >= 0) activatedBars[3] = _bars[_collidedBarIndex - 1];
        else activatedBars[1] = _bars[_collidedBarIndex];
        //Collided bar
        activatedBars[4] = _bars[_collidedBarIndex];
        //One bar to the right of the collided bar
        if (_collidedBarIndex + 1 < _bars.Count) activatedBars[5] = _bars[_collidedBarIndex + 1];
        else activatedBars[5] = _bars[_collidedBarIndex];
        //Two bars to the right of the collided bar
        if (_collidedBarIndex + 2 < _bars.Count) activatedBars[6] = _bars[_collidedBarIndex + 2];
        else activatedBars[6] = _bars[_collidedBarIndex];
        //Three bars to the right of the collided bar
        if (_collidedBarIndex + 3 < _bars.Count) activatedBars[7] = _bars[_collidedBarIndex + 3];
        else activatedBars[7] = _bars[_collidedBarIndex];
        //Four bars to the right of the collided bar
        if (_collidedBarIndex + 4 < _bars.Count) activatedBars[8] = _bars[_collidedBarIndex + 4];
        else activatedBars[8] = _bars[_collidedBarIndex];
    }

    private IEnumerator DesactivateActiveBars()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (Barra bar in GameManager.instance.barras)
        {
            if (bar.estado == Barra.Estado.estado_B)
            {
                bar.movimiento = Barra.Movimiento.automatico;
                bar.direccion = Barra.Direccion.sube;
                bar.GetValoresEstadoA();
            }
        }
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}