using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.Events;

public class Barra : MonoBehaviour
{
    public enum Movimiento { automatico, manual };
    public enum Direccion { sube, baja, sostenido }
    public enum Estado { estado_A, estado_B, transicion };

    [Header("Estados")]
    public BarreraData data;
    public Movimiento movimiento;
    public Direccion direccion;
    public Estado estado;
    public float alturaActual;
    public float heightTarget;
    public bool esProtagonista;

    [Space(10), Header("Basicos")]
    [Range(-5.5f, 4.5f)] public float alturaMin;
    [Range(-5.5f, 4.5f)] public float alturaMax;
    [Range(0, 20)] public float velocidadSubida;
    [Range(0, 20)] public float velocidadBajada;
    [Range(0, 0.5f)] public float rangoVariacionAltura;

    [Space(10)]
    [Header("Avanzados")]
    [Range(0, 2)] public float tiempoSostenidoEnMinimo;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo;


    Rigidbody2D rBody;
    //private float alturaMinimaPantalla = -8;
    [HideInInspector] public int index;
    [HideInInspector] public float variacionAltura;
    [HideInInspector] public UnityEvent AtLowestPoint = new UnityEvent();
    [HideInInspector] public UnityEvent AtHighestPoint = new UnityEvent();
    [HideInInspector] public UnityEvent StartGoingDown = new UnityEvent();
    [HideInInspector] public UnityEvent StartGoingUp = new UnityEvent();





    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetValoresEstadoA();
        index = GameManager.instance.barras.FindIndex(a => a == this);

        //Protagonista
        if (esProtagonista && GameManager.instance.protagonista == null)
        {
            GameManager.instance.protagonista = this;
            transform.name = "Protagonista";
        }

        //Debuggers
        if (alturaMin > alturaMax)
        {
            Debug.LogError("la altura min de " + transform.name + "no puede ser mayor que la altura max");
        }
    }

    private void FixedUpdate()
    {
        SetVariablesLimits();
        if (movimiento == Movimiento.manual) MovimientoManual();
        else StartCoroutine(MovimientoAutomatico());
    }


    public void MovimientoManual()
    {
        StopAllCoroutines();
        if (heightTarget < alturaMin) heightTarget = alturaMin;
        if (heightTarget > alturaMax) heightTarget = alturaMax;

        float factor;//0 teleport - 1 inmovil
        if (GameManager.instance.concepto == GameManager.Concepto.Acoso) factor = 0.5f;
        else if (GameManager.instance.concepto == GameManager.Concepto.Empatia) factor = 0.95f;
        else if (GameManager.instance.concepto == GameManager.Concepto.Vanidad && this == GameManager.instance.protagonista) factor = 0.5f;
        else if (GameManager.instance.concepto == GameManager.Concepto.Xenofobia) {
            if (index == 1 || index == 2) factor = 0.6f;
            else factor = 0.9f;
        }
        else if (GameManager.instance.concepto == GameManager.Concepto.Mediacion) factor = 0.9f;
        else factor = 0.9f;


        float newPosY = heightTarget * (1 - factor) + transform.position.y * factor;
        rBody.MovePosition(new Vector2(transform.position.x, newPosY));
    }


    public IEnumerator MovimientoAutomatico()
    {
        if (direccion == Direccion.sube)
        {
            //Tiempo sostenido en la altura maxima
            if (transform.position.y >= alturaMax + variacionAltura)
            {
                AtHighestPoint.Invoke();
                direccion = Direccion.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMaximo);
                direccion = Direccion.baja;
                StartGoingDown.Invoke();
                yield break;
            }
            //Movimiento 
            rBody.MovePosition(transform.position + (velocidadSubida * Vector3.up * Time.deltaTime));
            if (direccion == Direccion.baja) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }

        if (direccion == Direccion.baja)
        {
            //Tiempo sostenido en la altura minima
            if (transform.position.y <= alturaMin + (variacionAltura / 2))
            {
                AtLowestPoint.Invoke();
                direccion = Direccion.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMinimo);
                direccion = Direccion.sube;
                StartGoingUp.Invoke();
                yield break;
            }
            //Movimiento
            rBody.MovePosition(transform.position - (velocidadBajada * Vector3.up * Time.deltaTime));
            if (direccion == Direccion.sube) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }
        yield return null;
    }

    void SetVariablesLimits()
    {
        //Altura Minima
        if (data.alturaMin_B >= data.alturaMin_A)
        {
            alturaMin = Mathf.Clamp(alturaMin, data.alturaMin_A, data.alturaMin_B);
        }
        else alturaMin = Mathf.Clamp(alturaMin, data.alturaMin_B, data.alturaMin_A);

        //Altura Maxima
        if (data.alturaMax_B >= data.alturaMax_A)
        {
            alturaMax = Mathf.Clamp(alturaMax, data.alturaMax_A, data.alturaMax_B);
        }
        else alturaMax = Mathf.Clamp(alturaMax, data.alturaMax_B, data.alturaMax_A);
    }


    #region Botones de Editor
    [Button(Spacing = ButtonSpacing.Before)]
    public void GetValoresEstadoA()
    {
        alturaMin = data.alturaMin_A;
        alturaMax = data.alturaMax_A;
        velocidadSubida = data.velocidadSubida_A;
        velocidadBajada = data.velocidadBajada_A;
        rangoVariacionAltura = data.rangoVariacionAltura_A;
        tiempoSostenidoEnMinimo = data.tiempoSostenidoEnMinimo_A;
        tiempoSostenidoEnMaximo = data.tiempoSostenidoEnMaximo_A;

        //Variacion de altura
        if (rangoVariacionAltura == 0) variacionAltura = 0;
        else variacionAltura = Random.Range(0, rangoVariacionAltura);
        estado = Estado.estado_A;
    }

    [Button]
    public void GetValoresEstadoB()
    {
        alturaMin = data.alturaMin_B;
        alturaMax = data.alturaMax_B;
        velocidadSubida = data.velocidadSubida_B;
        velocidadBajada = data.velocidadBajada_B;
        rangoVariacionAltura = data.rangoVariacionAltura_B;
        tiempoSostenidoEnMinimo = data.tiempoSostenidoEnMinimo_B;
        tiempoSostenidoEnMaximo = data.tiempoSostenidoEnMaximo_B;

        //Variacion de altura
        if (rangoVariacionAltura == 0) variacionAltura = 0;
        else variacionAltura = Random.Range(0, rangoVariacionAltura);
        estado = Estado.estado_B;
    }

    [Button(Spacing = ButtonSpacing.Before)]
    void PosicionarEnAlturaMinima()
    {
        PosicionarHijo();
        transform.position = new Vector3(transform.position.x, alturaMin, transform.position.z);
    }

    [Button]
    void PosicionarEnAlturaMaxima()
    {
        PosicionarHijo();
        transform.position = new Vector3(transform.position.x, alturaMax, transform.position.z);

    }

    void PosicionarHijo()
    {
        Transform child = transform.GetChild(0).transform;
        child.localPosition = new Vector3(0, -4.5f, 0);
    }
    #endregion

    #region Funciones variadas
    public void GetValoresEstadoA_From(BarreraData _data)
    {
        alturaMin = _data.alturaMin_A;
        alturaMax = _data.alturaMax_A;
        velocidadSubida = _data.velocidadSubida_A;
        velocidadBajada = _data.velocidadBajada_A;
        rangoVariacionAltura = _data.rangoVariacionAltura_A;
        tiempoSostenidoEnMinimo = _data.tiempoSostenidoEnMinimo_A;
        tiempoSostenidoEnMaximo = _data.tiempoSostenidoEnMaximo_A;

        //Variacion de altura
        if (rangoVariacionAltura == 0) variacionAltura = 0;
        else variacionAltura = Random.Range(0, rangoVariacionAltura);
        estado = Estado.estado_A;
    }

    public void GetValoresEstadoB_From(BarreraData data)
    {
        alturaMin = data.alturaMin_B;
        alturaMax = data.alturaMax_B;
        velocidadSubida = data.velocidadSubida_B;
        velocidadBajada = data.velocidadBajada_B;
        rangoVariacionAltura = data.rangoVariacionAltura_B;
        tiempoSostenidoEnMinimo = data.tiempoSostenidoEnMinimo_B;
        tiempoSostenidoEnMaximo = data.tiempoSostenidoEnMaximo_B;

        //Variacion de altura
        if (rangoVariacionAltura == 0) variacionAltura = 0;
        else variacionAltura = Random.Range(0, rangoVariacionAltura);
        estado = Estado.estado_B;
    }
    #endregion
}
